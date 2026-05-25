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
    public class InventoryMovWriteRepository : IInventoryMovWriteRepository
    {
        private readonly string _connectionString;

        public InventoryMovWriteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default")
                ?? throw new InvalidOperationException("Connection string 'Default' not found.");
        }

        public async Task AddAsync(InventoryMov movement)
        {
            const string sql = @"
            INSERT INTO InventoryMovements (Id, ProductId, Quantity, MovementType, Reason, CreatedAt)
            VALUES (@Id, @ProductId, @Quantity, @MovementType, @Reason, @CreatedAt)";

            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(sql, new
            {
                movement.Id,
                movement.ProductId,
                movement.Quantity,
                MovementType = movement.MovementType.ToString(),
                movement.Reason,
                movement.CreatedAt
            });
        }
    }
}
