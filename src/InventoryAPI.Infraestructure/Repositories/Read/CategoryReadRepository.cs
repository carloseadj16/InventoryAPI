using InventoryAPI.Domain.Entities;
using InventoryAPI.Domain.Interfaces;
using InventoryAPI.Domain.Specifications;
using InventoryAPI.Infraestructure.Persistence;
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

        public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetAllAsync(ISpecification<Category> specification, CancellationToken cancellationToken)
        {
            return await SpecificationEvaluator<Category>.GetQuery(_context.Categories.AsNoTracking(), specification).ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(ISpecification<Category> specification, CancellationToken cancellationToken)
        {
            return await SpecificationEvaluator<Category>
                .GetQuery(_context.Categories.AsNoTracking(), specification)
                .CountAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Categories.AsNoTracking().AnyAsync(c => c.Id == id,cancellationToken);
        }
    }
}
