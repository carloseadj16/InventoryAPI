using InventoryAPI.Domain.Entities;
using InventoryAPI.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Domain.Interfaces
{
    public interface ICategoryReadRepository
    {
        Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Category>> GetAllAsync(ISpecification<Category> specification, CancellationToken cancellationToken);
        Task<int> CountAsync(ISpecification<Category> specification, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
    }
}
