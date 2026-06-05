using InventoryAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Domain.Interfaces
{
    public interface IIdempotencyRepository
    {
        Task<IdempotencyKey?> GetByRequestIdAsync(string requestId, CancellationToken cancellationToken);
        Task SaveAsync(IdempotencyKey idempotencyKey, CancellationToken cancellationToken);
    }
}
