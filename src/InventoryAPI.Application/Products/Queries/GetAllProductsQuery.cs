using InventoryAPI.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Application.Products.Queries
{
    public record GetAllProductsQuery(int Page, int PageSize, int? CategoryId = null) : IRequest<Result<PagedResult<ProductDto>>>;
}
