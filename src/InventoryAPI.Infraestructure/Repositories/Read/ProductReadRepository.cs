using InventoryAPI.Domain.Entities;
using InventoryAPI.Domain.Interfaces;
using InventoryAPI.Domain.Specifications;
using InventoryAPI.Infraestructure.Persistence;
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

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetAllAsync(ISpecification<Product> specification, CancellationToken cancellationToken)
        {
            return await SpecificationEvaluator<Product>
            .GetQuery(_context.Products.AsNoTracking(), specification)
            .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Products
                .AsNoTracking()
                .AnyAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<int> CountAsync(ISpecification<Product> specification, CancellationToken cancellationToken)
        {
            return await SpecificationEvaluator<Product>
                .GetQuery(_context.Products.AsNoTracking(), specification)
                .CountAsync(cancellationToken);
        }
    }
}
