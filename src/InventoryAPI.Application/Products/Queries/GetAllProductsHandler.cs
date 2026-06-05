using InventoryAPI.Application.Common;
using InventoryAPI.Domain.Interfaces;
using InventoryAPI.Domain.Specifications.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Application.Products.Queries
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, Result<PagedResult<ProductDto>>>
    {
        private readonly IProductReadRepository _readRepository;

        public GetAllProductsHandler(IProductReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task<Result<PagedResult<ProductDto>>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
        {
            var specification = new PagedProductsSpecification(query.Page, query.PageSize, query.CategoryId);
            var totalSpec = new ActiveProductsSpecification();
            var products = await _readRepository.GetAllAsync(specification, cancellationToken);
            var totalCount = await _readRepository.CountAsync(totalSpec,cancellationToken);
            var dtos = products.Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.Stock,
                p.CategoryId,
                p.CreatedAt,
                p.UpdatedAt));
            var pagedResult = PagedResult<ProductDto>.Create(dtos, query.Page, query.PageSize, totalCount);

            return Result<PagedResult<ProductDto>>.Success(pagedResult);
        }
    }
}
