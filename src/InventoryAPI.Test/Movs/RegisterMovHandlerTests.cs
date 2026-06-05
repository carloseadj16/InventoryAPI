using FluentAssertions;
using InventoryAPI.Application.Movs.Commands;
using InventoryAPI.Domain.Entities;
using InventoryAPI.Domain.Enums;
using InventoryAPI.Domain.Interfaces;
using Moq;

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
            string requestId = Guid.NewGuid().ToString();
            var product = Product.Create("Widget", "Desc", 5m, 10,1);
            var command = new RegisterMovCommand(requestId,productId, 5, MovementType.In, "Purchase");
            _productReadRepositoryMock.Setup(r => r.GetByIdAsync(productId,CancellationToken.None)).ReturnsAsync(product);
            _productWriteRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _movementWriteRepositoryMock.Setup(r => r.AddAsync(It.IsAny<InventoryMov>())).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            _movementWriteRepositoryMock.Verify(r => r.AddAsync(It.IsAny<InventoryMov>()), Times.Once);
        }

    }
}
