using InventoryAPI.Domain.Entities;
using InventoryAPI.Domain.Interfaces;
using InventoryAPI.Infraestructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Infraestructure.Repositories.Read
{
    public class CategoryReadRepository : ICategoryReadRepository
    {
        private readonly InventoryDbContext _context;

        public CategoryReadRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.AsNoTracking().ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Categories.AsNoTracking().AnyAsync(c => c.Id == id);
        }
    }
}
