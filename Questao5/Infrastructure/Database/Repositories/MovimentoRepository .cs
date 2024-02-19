using System.Data;
using Dapper;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.Sqlite;
using Questao5.Application.Commands.Requests;
using Questao5.Infrastructure.Database.Repositories.Interfaces;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.Repositories
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly DatabaseConfig _databaseConfig;

        public MovimentoRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async Task<string> SalvarMovimento(MovimentoRequestDto movimentoDto)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            await connection.OpenAsync();

            var query = @"INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) 
                          VALUES (@MovimentoId, @ContaCorrenteId, @Data, @TipoMovimento, @Valor);
                          SELECT @MovimentoId";

            var parameters = new
            {
                MovimentoId = Guid.NewGuid().ToString(),
                ContaCorrenteId = movimentoDto.IdContaCorrente,
                Data = DateTime.Now,
                TipoMovimento = movimentoDto.TipoMovimento,
                Valor = movimentoDto.Valor
            };

            var movementId = await connection.ExecuteScalarAsync<string>(query, parameters);
            return movementId;
        }

        public async Task<decimal> GetSaldo(string idConta)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            await connection.OpenAsync();

            var query = @"SELECT 
                            COALESCE(SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE 0 END), 0) - 
                            COALESCE(SUM(CASE WHEN tipomovimento = 'D' THEN valor ELSE 0 END), 0) AS Saldo
                          FROM movimento 
                          WHERE idcontacorrente = @idConta";

            var saldo = await connection.ExecuteScalarAsync<decimal>(query, new { idConta });

            return saldo;
        }
    }
}
