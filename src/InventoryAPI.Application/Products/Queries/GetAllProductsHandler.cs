using InventoryAPI.Application.Common;
using InventoryAPI.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Application.Products.Queries
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, Result<IEnumerable<ProductDto>>>
    {
        private readonly IProductReadRepository _readRepository;

        public GetAllProductsHandler(IProductReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task<Result<IEnumerable<ProductDto>>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
        {
            var products = await _readRepository.GetAllAsync();
            var dtos = products.Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.Stock,
                p.CategoryId,
                p.CreatedAt,
                p.UpdatedAt));

            return Result<IEnumerable<ProductDto>>.Success(dtos);
        }
    }
}
