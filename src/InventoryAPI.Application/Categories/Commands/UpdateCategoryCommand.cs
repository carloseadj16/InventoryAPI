using InventoryAPI.Application.Common;
using InventoryAPI.Domain.Entities;
using MediatR;

namespace InventoryAPI.Application.Categories.Commands
{
    public record UpdateCategoryCommand(int Id, string Name, string Description) : IRequest<Result<bool>>;

}
