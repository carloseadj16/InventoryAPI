using FluentAssertions;
using InventoryAPI.Application.Movs.Commands;
using InventoryAPI.Domain.Entities;
using InventoryAPI.Domain.Enums;
using InventoryAPI.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Test.Movs
{
    public class RegisterMovHandlerTests
    {
        private readonly Mock<IProductReadRepository> _productReadRepositoryMock;
        private readonly Mock<IProductWriteRepository> _productWriteRepositoryMock;
        private readonly Mock<IInventoryMovWriteRepository> _movementWriteRepositoryMock;
        private readonly RegisterMovHandler _handler;

        public RegisterMovHandlerTests()
        {
            _productReadRepositoryMock = new Mock<IProductReadRepository>();
            _productWriteRepositoryMock = new Mock<IProductWriteRepository>();
            _movementWriteRepositoryMock = new Mock<IInventoryMovWriteRepository>();
            _handler = new RegisterMovHandler(
                _productReadRepositoryMock.Object,
                _productWriteRepositoryMock.Object,
                _movementWriteRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenStockInIsRegistered()
        {
            var productId = 1;
            var product = Product.Create("Widget", "Desc", 5m, 10,1);
            var command = new RegisterMovCommand(productId, 5, MovementType.In, "Purchase");
            _productReadRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
            _productWriteRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
            _movementWriteRepositoryMock.Setup(r => r.AddAsync(It.IsAny<InventoryMov>())).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            _movementWriteRepositoryMock.Verify(r => r.AddAsync(It.IsAny<InventoryMov>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenProductDoesNotExist()
        {
            var productId = 1;
            var command = new RegisterMovCommand(productId, 5, MovementType.In, "Purchase");
            _productReadRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync((Product?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Product not found.");
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenStockOutExceedsAvailable()
        {
            var productId = 1;
            var product = Product.Create("Widget", "Desc", 5m, 3, 1);
            var command = new RegisterMovCommand(productId, 10, MovementType.Out, "Sale");
            _productReadRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Insufficient stock for this operation.");
        }

        [Fact]
        public async Task Handle_ShouldAdjustStock_WhenMovementOutIsValid()
        {
            var productId = 1;
            var product = Product.Create("Widget", "Desc", 5m, 10, 1);
            var command = new RegisterMovCommand(productId, 4, MovementType.Out, "Sale");
            _productReadRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
            _productWriteRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
            _movementWriteRepositoryMock.Setup(r => r.AddAsync(It.IsAny<InventoryMov>())).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            product.Stock.Should().Be(6);
        }
    }
}
