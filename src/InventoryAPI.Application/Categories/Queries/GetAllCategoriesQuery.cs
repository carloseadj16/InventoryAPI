using InventoryAPI.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Application.Categories.Queries
{
    public record GetAllCategoriesQuery : IRequest<Result<IEnumerable<CategoryDto>>>;
}
