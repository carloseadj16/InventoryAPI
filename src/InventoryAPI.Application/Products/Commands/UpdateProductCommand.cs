using InventoryAPI.Application.Common;
using InventoryAPI.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Application.Products.Commands
{
    public record UpdateProductCommand(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int CategoryId
) : IRequest<Result<bool>>;
}
