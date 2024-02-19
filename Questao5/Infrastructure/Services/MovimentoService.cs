using Questao5.Application.Commands;
using Questao5.Application.Commands.Requests;
using Questao5.Infrastructure.Database.Repositories.Interfaces;
using Questao5.Infrastructure.Services.Interfaces;

namespace Questao5.Infrastructure.Services
{
    public class MovimentoService : IMovimentoService
    {
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IIdempotenciaRepository _idempotenciaRepository;

        public MovimentoService(IMovimentoRepository movimentoRepository, IContaCorrenteRepository contaCorrenteRepository, IIdempotenciaRepository idempotenciaRepository)
        {
            _movimentoRepository = movimentoRepository;
            _contaCorrenteRepository = contaCorrenteRepository;
            _idempotenciaRepository = idempotenciaRepository;
        }

        public async Task<string> GerarMovimentoAsync(MovimentoRequestDto movimentoDto)
        {
            var idempotentRequest = _idempotenciaRepository.GetStoredResponse(movimentoDto.RequestId);

            if (idempotentRequest != null)
            {
                return idempotentRequest.resultado;
            } else
            {
                await ValidaMovimentoRequest(movimentoDto);

                var movementId = await _movimentoRepository.SalvarMovimento(movimentoDto);

                var idempotentResponse = new IdempotenciaRequestResponse
                {
                    chave_idempotencia = movimentoDto.RequestId,
                    requisicao = movimentoDto.ToString(),
                    resultado = movementId
                };
                _idempotenciaRepository.StoreResponse(idempotentResponse);

                return movementId;
            }
        }

        private async Task ValidaMovimentoRequest(MovimentoRequestDto movimentoDto)
        {
            if (movimentoDto == null)
            {
                throw new ArgumentException("Request de movimento não pode ser nulo");
            }

            var contaCorrente = await _contaCorrenteRepository.GetContaCorrente(movimentoDto.IdContaCorrente);

            if (contaCorrente == null)
            {
                throw new ArgumentException("Conta Invalida: Conta não encontrada.", "INVALID_ACCOUNT");
            }

            if (!contaCorrente.Ativo)
            {
                throw new ArgumentException("Conta Inativa: A conta não está ativa.", "INACTIVE_ACCOUNT");
            }

            if (movimentoDto.Valor <= 0)
            {
                throw new ArgumentException("Apenas valores positivos podem ser recebidos", "INVALID_VALUE");
            }

            if (movimentoDto.TipoMovimento is not 'C' and not 'D')
            {
                throw new ArgumentException("Apenas tipo Crédito e Débito são aceitos", "INVALID_TYPE");
            }
        }
    }
}
