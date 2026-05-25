using InventoryAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Domain.Entities
{
    public class InventoryMov
    {
        public int Id { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public MovementType MovementType { get; private set; }
        public string Reason { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }

        private InventoryMov() { }

        public static InventoryMov Create(int productId, int quantity, MovementType movementType, string reason)
        {
            return new InventoryMov
            {
                ProductId = productId,
                Quantity = quantity,
                MovementType = movementType,
                Reason = reason,
                CreatedAt = DateTime.UtcNow
            };
        }

    }
}
