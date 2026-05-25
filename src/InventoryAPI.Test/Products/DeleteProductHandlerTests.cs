using FluentAssertions;
using InventoryAPI.Application.Products.Commands;
using InventoryAPI.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Test.Products
{
    public class DeleteProductHandlerTests
    {
        private readonly Mock<IProductReadRepository> _readRepositoryMock;
        private readonly Mock<IProductWriteRepository> _writeRepositoryMock;
        private readonly DeleteProductHandler _handler;

        public DeleteProductHandlerTests()
        {
            _readRepositoryMock = new Mock<IProductReadRepository>();
            _writeRepositoryMock = new Mock<IProductWriteRepository>();
            _handler = new DeleteProductHandler(_readRepositoryMock.Object, _writeRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenProductExists()
        {
            var productId = 1;
            _readRepositoryMock.Setup(r => r.ExistsAsync(productId)).ReturnsAsync(true);
            _writeRepositoryMock.Setup(r => r.DeleteAsync(productId)).Returns(Task.CompletedTask);

            var result = await _handler.Handle(new DeleteProductCommand(productId), CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            _writeRepositoryMock.Verify(r => r.DeleteAsync(productId), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenProductDoesNotExist()
        {
            var productId = 1;
            _readRepositoryMock.Setup(r => r.ExistsAsync(productId)).ReturnsAsync(false);

            var result = await _handler.Handle(new DeleteProductCommand(productId), CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Product not found.");
            _writeRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
