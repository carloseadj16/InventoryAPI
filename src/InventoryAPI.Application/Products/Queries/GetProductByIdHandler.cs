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
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
    {
        private readonly IProductReadRepository _readRepository;

        public GetProductByIdHandler(IProductReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task<Result<ProductDto>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await _readRepository.GetByIdAsync(query.Id, cancellationToken);
            if (product is null)
                return Result<ProductDto>.Failure("Product not found.");

            var dto = new ProductDto(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Stock,
                product.CategoryId,
                product.CreatedAt,
                product.UpdatedAt);

            return Result<ProductDto>.Success(dto);
        }
    }
}
