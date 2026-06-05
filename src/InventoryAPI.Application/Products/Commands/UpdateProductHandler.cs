using InventoryAPI.Application.Common;
using InventoryAPI.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Application.Products.Commands
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Result<bool>>
    {
        private readonly IProductReadRepository _readRepository;
        private readonly IProductWriteRepository _writeRepository;
        private readonly ICategoryReadRepository _categoryReadRepository;

        public UpdateProductHandler(
            IProductReadRepository readRepository,
            IProductWriteRepository writeRepository,
            ICategoryReadRepository categoryReadRepository)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _categoryReadRepository = categoryReadRepository;
        }

        public async Task<Result<bool>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _readRepository.GetByIdAsync(command.Id, cancellationToken);
            if (product is null)
                return Result<bool>.Failure("Product not found.");

            var categoryExists = await _categoryReadRepository.ExistsAsync(command.CategoryId, cancellationToken);
            if (!categoryExists)
                return Result<bool>.Failure("Category not found.");

            product.Update(command.Name, command.Description, command.Price, command.CategoryId);
            await _writeRepository.UpdateAsync(product, cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
