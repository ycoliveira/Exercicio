using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.Commands;
using Questao5.Application.Commands.Requests;
using Questao5.Infrastructure.Database.Repositories.Interfaces;
using Questao5.Infrastructure.Sqlite;
using System.Text.RegularExpressions;

namespace Questao5.Infrastructure.Database.Repositories
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly DatabaseConfig _databaseConfig;

        public IdempotenciaRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public IdempotenciaRequestResponse GetStoredResponse(string requestId)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);

            return connection.QueryFirstOrDefault<IdempotenciaRequestResponse>(
                "SELECT * FROM idempotencia WHERE chave_idempotencia = @RequestId",
                new { RequestId = requestId }
            );
        }

        public void StoreResponse(IdempotenciaRequestResponse request)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            connection.Execute(
                @"INSERT OR REPLACE INTO idempotencia (chave_idempotencia , requisicao, resultado) 
              VALUES (@RequestId, @RequestBody, @ResponseBody)",
                new
                {
                    RequestId = request.chave_idempotencia,
                    RequestBody = request.requisicao,
                    ResponseBody = request.resultado
                }
            );
        }
    }
}
