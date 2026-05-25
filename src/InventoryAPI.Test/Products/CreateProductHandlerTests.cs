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
    public class CreateProductHandlerTests
    {
        private readonly Mock<IProductWriteRepository> _writeRepositoryMock;
        private readonly Mock<ICategoryReadRepository> _categoryReadRepositoryMock;
        private readonly CreateProductHandler _handler;

        public CreateProductHandlerTests()
        {
            _writeRepositoryMock = new Mock<IProductWriteRepository>();
            _categoryReadRepositoryMock = new Mock<ICategoryReadRepository>();
            _handler = new CreateProductHandler(_writeRepositoryMock.Object, _categoryReadRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenCategoryExists()
        {
            var categoryId = 1;
            var command = new CreateProductCommand("Widget", "A test widget", 9.99m, 10, categoryId);
            _categoryReadRepositoryMock.Setup(r => r.ExistsAsync(categoryId)).ReturnsAsync(true);
            _writeRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBe(0);
            _writeRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCategoryDoesNotExist()
        {
            var categoryId = 1;
            var command = new CreateProductCommand("Widget", "A test widget", 9.99m, 10, categoryId);
            _categoryReadRepositoryMock.Setup(r => r.ExistsAsync(categoryId)).ReturnsAsync(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Category not found.");
            _writeRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Never);
        }
    }
}
