using InventoryAPI.Domain.Enums;

namespace InventoryAPI.Controllers.Request
{
    public record RegisterMovementRequest(
        int ProductId,
        int Quantity,
        MovementType MovementType,
        string Reason
    );
}
