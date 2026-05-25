using FluentAssertions;
using InventoryAPI.Application.Categories.Commands;
using InventoryAPI.Domain.Entities;
using InventoryAPI.Domain.Interfaces;
using Moq;

namespace InventoryAPI.Test.Categories
{
    public class CreateCategoryHandlerTests
    {
        private readonly Mock<ICategoryWriteRepository> _writeRepositoryMock;
        private readonly CreateCategoryHandler _handler;

        public CreateCategoryHandlerTests()
        {
            _writeRepositoryMock = new Mock<ICategoryWriteRepository>();
            _handler = new CreateCategoryHandler(_writeRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WithNewGuid()
        {
            var command = new CreateCategoryCommand("Electronics", "Electronic products");
            _writeRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBe(0);
            _writeRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCallAddAsync_WithCorrectCategoryData()
        {
            Category? capturedCategory = null;
            var command = new CreateCategoryCommand("Electronics", "Electronic products");
            _writeRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Category>()))
                .Callback<Category>(c => capturedCategory = c)
                .Returns(Task.CompletedTask);

            await _handler.Handle(command, CancellationToken.None);

            capturedCategory.Should().NotBeNull();
            capturedCategory!.Name.Should().Be("Electronics");
            capturedCategory.Description.Should().Be("Electronic products");
        }
    }
}
