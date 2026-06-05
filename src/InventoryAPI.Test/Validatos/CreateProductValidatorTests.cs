using FluentAssertions;
using InventoryAPI.Application.Products.Commands;

namespace InventoryAPI.Test.Validatos
{
    public class CreateProductValidatorTests
    {
        private readonly CreateProductValidator _validator = new();

        [Fact]
        public void Validate_ShouldPass_WhenCommandIsValid()
        {
            var command = new CreateProductCommand("CCEEFFRA","Laptop", "Description", 999.99m, 10, 1);

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_ShouldFail_WhenNameIsEmpty()
        {
            var command = new CreateProductCommand("AbCd56","Laptop", "Description", 999.99m, 10, 1);

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
        }

        [Fact]
        public void Validate_ShouldFail_WhenPriceIsZero()
        {
            var command = new CreateProductCommand("Axb23hhh","Laptop", "Description", 0m, 10, 1);

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Price");
        }

        [Fact]
        public void Validate_ShouldFail_WhenStockIsNegative()
        {
            var command = new CreateProductCommand("Abc12rtF","Laptop", "Description", 999.99m, -1, 1);

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Stock");
        }

        [Fact]
        public void Validate_ShouldFail_WhenCategoryIdIsZero()
        {
            var command = new CreateProductCommand("AbC4a2","Laptop", "Description", 999.99m, 10, 0);

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CategoryId");
        }
    }
}
