using Dapper;
using InventoryAPI.Domain.Entities;
using InventoryAPI.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace InventoryAPI.Infraestructure.Repositories.Write
{
    public class CategoryWriteRepository : ICategoryWriteRepository
    {
        private readonly string _connectionString;

        public CategoryWriteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default")
                ?? throw new InvalidOperationException("Connection string 'Default' not found.");
        }

        public async Task<int> AddAsync(Category category)
        {
            const string sql = @"
            INSERT INTO Categories (Name, Description, CreatedAt, Active)
            VALUES (@Name, @Description, @CreatedAt, 1);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                category.Name,
                category.Description,
                category.CreatedAt
            });
        }

        public async Task UpdateAsync(Category category)
        {
            const string sql = @"
            UPDATE Categories
            SET Name = @Name, Description = @Description, UpdatedAt = @UpdatedAt
            WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(sql, new
            {
                category.Id,
                category.Name,
                category.Description,
                category.UpdatedAt
            });
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = "UPDATE Categories SET Active = 0, UpdatedAt = GETDATE()  WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
