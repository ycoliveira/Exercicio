using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.Repositories.Interfaces;
using Questao5.Infrastructure.Sqlite;
using System.Data;

namespace Questao5.Infrastructure.Database.Repositories
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly DatabaseConfig _databaseConfig;

        public ContaCorrenteRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public IEnumerable<ContaCorrente> GetcontasCorrentesAtivas()
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            connection.Open();
            string query = "SELECT * FROM contacorrente WHERE ativo = 1";
            return connection.Query<ContaCorrente>(query);
        }

        public async Task<ContaCorrente> GetContaCorrente(string idConta)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);

            var query = "SELECT * FROM contacorrente WHERE idcontacorrente = @IdConta";

            return await connection.QueryFirstOrDefaultAsync<ContaCorrente>(query, new { IdConta = idConta });
        }
    }
}
