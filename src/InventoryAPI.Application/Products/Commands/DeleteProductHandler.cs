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
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Result<bool>>
    {
        private readonly IProductReadRepository _readRepository;
        private readonly IProductWriteRepository _writeRepository;

        public DeleteProductHandler(IProductReadRepository readRepository, IProductWriteRepository writeRepository)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
        }

        public async Task<Result<bool>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var exists = await _readRepository.ExistsAsync(command.Id);
            if (!exists)
                return Result<bool>.Failure("Product not found.");

            await _writeRepository.DeleteAsync(command.Id);

            return Result<bool>.Success(true);
        }
    }
}
