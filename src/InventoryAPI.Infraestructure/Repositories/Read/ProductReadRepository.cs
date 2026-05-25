using InventoryAPI.Domain.Entities;
using InventoryAPI.Domain.Interfaces;
using InventoryAPI.Infraestructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Infraestructure.Repositories.Read
{
    public class ProductReadRepository : IProductReadRepository
    {
        private readonly InventoryDbContext _context;

        public ProductReadRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.AsNoTracking().ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products.AsNoTracking().AnyAsync(p => p.Id == id);
        }
    }
}
