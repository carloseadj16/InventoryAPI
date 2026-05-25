using FluentAssertions;
using InventoryAPI.Application.Products.Commands;
using InventoryAPI.Domain.Entities;
using InventoryAPI.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Test.Products
{
    public class UpdateProductHandlerTests
    {
        private readonly Mock<IProductReadRepository> _readRepositoryMock;
        private readonly Mock<IProductWriteRepository> _writeRepositoryMock;
        private readonly Mock<ICategoryReadRepository> _categoryReadRepositoryMock;
        private readonly UpdateProductHandler _handler;

        public UpdateProductHandlerTests()
        {
            _readRepositoryMock = new Mock<IProductReadRepository>();
            _writeRepositoryMock = new Mock<IProductWriteRepository>();
            _categoryReadRepositoryMock = new Mock<ICategoryReadRepository>();
            _handler = new UpdateProductHandler(
                _readRepositoryMock.Object,
                _writeRepositoryMock.Object,
                _categoryReadRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenProductAndCategoryExist()
        {
            var productId = 1;
            var categoryId = 1;
            var existingProduct = Product.Create("Old Name", "Old Desc", 1.0m, 5, categoryId);
            var command = new UpdateProductCommand(productId, "New Name", "New Desc", 2.0m, categoryId);
            _readRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(existingProduct);
            _categoryReadRepositoryMock.Setup(r => r.ExistsAsync(categoryId)).ReturnsAsync(true);
            _writeRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            _writeRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenProductDoesNotExist()
        {
            var productId = 1;
            var command = new UpdateProductCommand(productId, "Name", "Desc", 1.0m, 1);
            _readRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync((Product?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Product not found.");
        }
    }
}
