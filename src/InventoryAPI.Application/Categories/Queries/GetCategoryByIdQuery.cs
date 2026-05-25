using InventoryAPI.Application.Common;
using MediatR;

namespace InventoryAPI.Application.Categories.Queries
{
    public record GetCategoryByIdQuery(int Id) : IRequest<Result<CategoryDto>>;
}
