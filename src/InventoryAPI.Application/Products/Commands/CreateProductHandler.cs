using InventoryAPI.Application.Common;
using InventoryAPI.Domain.Entities;
using InventoryAPI.Domain.Exceptions;
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
            var categoryExists = await _categoryReadRepository.ExistsAsync(command.CategoryId, cancellationToken);
            if (!categoryExists)
                throw new NotFoundException("Category", command.CategoryId);

            var product = Product.Create(command.Name, command.Description, command.Price, command.Stock, command.CategoryId);
            var Id = await _writeRepository.AddAsync(product, cancellationToken);

            return Result<int>.Success(Id);
        }
    }
}
