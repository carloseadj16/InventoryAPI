using InventoryAPI.Application.Common;
using InventoryAPI.Domain.Entities;
using InventoryAPI.Domain.Enums;
using InventoryAPI.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Application.Movs.Commands
{
    public class RegisterMovHandler : IRequestHandler<RegisterMovCommand, Result<int>>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IInventoryMovWriteRepository _movementWriteRepository;

        public RegisterMovHandler(
            IProductReadRepository productReadRepository,
            IProductWriteRepository productWriteRepository,
            IInventoryMovWriteRepository movementWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _movementWriteRepository = movementWriteRepository;
        }

        public async Task<Result<int>> Handle(RegisterMovCommand command, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetByIdAsync(command.ProductId);
            if (product is null)
                return Result<int>.Failure("Product not found.");

            var stockDelta = command.MovementType == MovementType.In ? command.Quantity : -command.Quantity;
            if (product.Stock + stockDelta < 0)
                return Result<int>.Failure("Insufficient stock for this operation.");


            await _productWriteRepository.UpdateAsync(product);

            var movement = InventoryMov.Create(command.ProductId, command.Quantity, command.MovementType, command.Reason);
            await _movementWriteRepository.AddAsync(movement);

            return Result<int>.Success(movement.Id);
        }
    }
}
