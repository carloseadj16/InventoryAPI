namespace InventoryAPI.Controllers.Request
{
    public record CreateProductRequest(
        string Name,
        string Description,
        decimal Price,
        int Stock,
        int CategoryId
    );
}
