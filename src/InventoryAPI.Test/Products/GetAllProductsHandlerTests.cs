using FluentAssertions;
using InventoryAPI.Application.Products.Queries;
using InventoryAPI.Domain.Entities;
using InventoryAPI.Domain.Interfaces;
using InventoryAPI.Domain.Specifications;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Test.Products
{
    public class GetAllProductsHandlerTests
    {
        private readonly Mock<IProductReadRepository> _readRepositoryMock;
        private readonly GetAllProductsHandler _handler;

        public GetAllProductsHandlerTests()
        {
            _readRepositoryMock = new Mock<IProductReadRepository>();
            _handler = new GetAllProductsHandler(_readRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllProducts_WhenProductsExist()
        {
            var products = new List<Product>
            {
                Product.Create("Product A", "Desc A", 10m, 5, 1),
                Product.Create("Product B", "Desc B", 20m, 3, 2)
            };
            _readRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>())).ReturnsAsync(products);

            var result = await _handler.Handle(new GetAllProductsQuery(1,20), CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Items.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            _readRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Product>());

            var result = await _handler.Handle(new GetAllProductsQuery(1,20), CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Items.Should().BeEmpty();
        }
    }
}
