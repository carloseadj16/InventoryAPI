using InventoryAPI.Application.Common;
using InventoryAPI.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Application.Categories.Queries
{
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDto>>
    {
        private readonly ICategoryReadRepository _readRepository;

        public GetCategoryByIdHandler(ICategoryReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
        {
            var category = await _readRepository.GetByIdAsync(query.Id, cancellationToken);
            if (category is null)
                return Result<CategoryDto>.Failure("Category not found.");

            var dto = new CategoryDto(
                category.Id,
                category.Name,
                category.Description,
                category.CreatedAt,
                category.UpdatedAt);

            return Result<CategoryDto>.Success(dto);
        }
    }
}
