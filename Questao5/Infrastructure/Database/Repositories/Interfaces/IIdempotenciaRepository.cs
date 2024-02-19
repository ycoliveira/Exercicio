using Questao5.Application.Commands;

namespace Questao5.Infrastructure.Database.Repositories.Interfaces
{
    public interface IIdempotenciaRepository
    {
        IdempotenciaRequestResponse GetStoredResponse(string requestId);
        void StoreResponse(IdempotenciaRequestResponse request);
    }
}
