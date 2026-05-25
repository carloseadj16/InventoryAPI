using InventoryAPI.Application.Common;
using InventoryAPI.Domain.Entities;
using InventoryAPI.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Application.Products.Commands
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<int>>
    {
        private readonly IProductWriteRepository _writeRepository;
        private readonly ICategoryReadRepository _categoryReadRepository;

        public CreateProductHandler(IProductWriteRepository writeRepository, ICategoryReadRepository categoryReadRepository)
        {
            _writeRepository = writeRepository;
            _categoryReadRepository = categoryReadRepository;
        }

        public async Task<Result<int>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var categoryExists = await _categoryReadRepository.ExistsAsync(command.CategoryId);
            if (!categoryExists)
                return Result<int>.Failure("Category not found.");

            var product = Product.Create(command.Name, command.Description, command.Price, command.Stock, command.CategoryId);
            await _writeRepository.AddAsync(product);

            return Result<int>.Success(product.Id);
        }
    }
}
