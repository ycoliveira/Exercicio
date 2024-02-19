using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.Repositories.Interfaces
{
    public interface IContaCorrenteRepository
    {
        IEnumerable<ContaCorrente> GetcontasCorrentesAtivas();
        Task<ContaCorrente> GetContaCorrente(string idConta);
    }
}
