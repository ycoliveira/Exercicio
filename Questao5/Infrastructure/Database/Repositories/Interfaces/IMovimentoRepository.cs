using Questao5.Application.Commands.Requests;

namespace Questao5.Infrastructure.Database.Repositories.Interfaces
{
    public interface IMovimentoRepository
    {
        Task<string> SalvarMovimento(MovimentoRequestDto movimentoDto);
        Task<decimal> GetSaldo(string accountId);
    }
}
