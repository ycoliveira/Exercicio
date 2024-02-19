using Questao5.Application.Commands.Responses;
using Questao5.Infrastructure.Database.Repositories.Interfaces;
using Questao5.Infrastructure.Services.Interfaces;

namespace Questao5.Infrastructure.Services
{
    public class SaldoService : ISaldoService
    {
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IContaCorrenteRepository _contaCorrenteRepository;

        public SaldoService(IMovimentoRepository movimentoRepository, IContaCorrenteRepository contaCorrenteRepository)
        {
            _movimentoRepository = movimentoRepository;
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public async Task<SaldoResponseDto> GetSaldo(string idConta)
        {
            var contaCorrente = await _contaCorrenteRepository.GetContaCorrente(idConta);

            if (contaCorrente == null)
            {
                throw new ArgumentException("Conta Invalida: Conta não encontrada.", "INVALID_ACCOUNT");
            }

            if (!contaCorrente.Ativo)
            {
                throw new ArgumentException("Conta Inativa: A conta não está ativa.", "INACTIVE_ACCOUNT");
            }

            var saldo = await _movimentoRepository.GetSaldo(idConta);

            var response = new SaldoResponseDto
            {
                NumeroContaCorrente = contaCorrente.Numero,
                NomeTitular = contaCorrente.Nome,
                DataResponse = DateTime.Now,
                SaldoAtual = saldo
            };

            return response;
        }
    }
}
