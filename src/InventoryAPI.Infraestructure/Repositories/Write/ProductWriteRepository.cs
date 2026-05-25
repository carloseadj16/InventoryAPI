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
    public class ProductWriteRepository : IProductWriteRepository
    {
        private readonly string _connectionString;

        public ProductWriteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default")
                ?? throw new InvalidOperationException("Connection string 'Default' not found.");
        }

        public async Task AddAsync(Product product)
        {
            const string sql = @"
            INSERT INTO Products (Name, Description, Price, Stock, CategoryId, CreatedAt)
            VALUES (@Name, @Description, @Price, @Stock, @CategoryId, @CreatedAt)";

            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(sql, new
            {
                product.Name,
                product.Description,
                product.Price,
                product.Stock,
                product.CategoryId,
                product.CreatedAt
            });
        }

        public async Task UpdateAsync(Product product)
        {
            const string sql = @"
            UPDATE Products
            SET Name = @Name, Description = @Description, Price = @Price,
                Stock = @Stock, CategoryId = @CategoryId, UpdatedAt = @UpdatedAt
            WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(sql, new
            {
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Stock,
                product.CategoryId,
                product.UpdatedAt
            });
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Products WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
