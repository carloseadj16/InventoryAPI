using InventoryAPI.Application.Common;
using InventoryAPI.Domain.Interfaces;
using InventoryAPI.Domain.Specifications.Categories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Application.Categories.Queries
{
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, Result<IEnumerable<CategoryDto>>>
    {
        private readonly ICategoryReadRepository _readRepository;

        public GetAllCategoriesHandler(ICategoryReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task<Result<IEnumerable<CategoryDto>>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
        {
            var spespecification = new ActiveCategoriesSpecification();
            var categories = await _readRepository.GetAllAsync(spespecification,cancellationToken);
            var dtos = categories.Select(c => new CategoryDto(
                c.Id,
                c.Name,
                c.Description,
                c.CreatedAt,
                c.UpdatedAt));

            return Result<IEnumerable<CategoryDto>>.Success(dtos);
        }
    }
}
