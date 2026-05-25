using InventoryAPI.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Application.Products.Commands
{
    public record DeleteProductCommand(int Id) : IRequest<Result<bool>>;
}
