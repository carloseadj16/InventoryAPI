using InventoryAPI.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Application.Products.Queries
{
    public record GetAllProductsQuery : IRequest<Result<IEnumerable<ProductDto>>>;
}
