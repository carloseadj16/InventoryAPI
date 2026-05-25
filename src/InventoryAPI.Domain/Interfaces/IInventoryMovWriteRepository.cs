using InventoryAPI.Domain.Entities;

namespace InventoryAPI.Domain.Interfaces
{
    public interface IInventoryMovWriteRepository
    {
        Task AddAsync(InventoryMov movement);
    }
}
