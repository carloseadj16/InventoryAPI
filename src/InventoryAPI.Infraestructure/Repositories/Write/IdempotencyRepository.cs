using Dapper;
using InventoryAPI.Domain.Entities;
using InventoryAPI.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Infraestructure.Repositories.Write
{
    public class IdempotencyRepository : IIdempotencyRepository
    {
        private readonly string _connectionString;

        public IdempotencyRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default")
                ?? throw new InvalidOperationException("Connection string 'Default' not found.");
        }

        public async Task<IdempotencyKey?> GetByRequestIdAsync(string requestId, CancellationToken cancellationToken)
        {
            const string sql = "SELECT Id, RequestId, Response, CreatedAt FROM IdempotencyKeys WHERE RequestId = @RequestId";

            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryFirstOrDefaultAsync<IdempotencyKey>(
                new CommandDefinition(sql, new { RequestId = requestId }, cancellationToken: cancellationToken));
        }

        public async Task SaveAsync(IdempotencyKey idempotencyKey, CancellationToken cancellationToken)
        {
            const string sql = @"
            INSERT INTO IdempotencyKeys (Id, RequestId, Response, CreatedAt)
            VALUES (@Id, @RequestId, @Response, @CreatedAt)";

            using var connection = new SqlConnection(_connectionString);

            await connection.ExecuteAsync(
                new CommandDefinition(sql, new
                {
                    idempotencyKey.Id,
                    idempotencyKey.RequestId,
                    idempotencyKey.Response,
                    idempotencyKey.CreatedAt
                }, cancellationToken: cancellationToken));
        }
    }
}
