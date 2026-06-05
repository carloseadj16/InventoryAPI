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
    public record CreateProductCommand(
        string RequestId,
        string Name,
        string Description,
        decimal Price,
        int Stock,
        int CategoryId
    ) : IRequest<Result<int>>, IIdempotentCommand;
}
