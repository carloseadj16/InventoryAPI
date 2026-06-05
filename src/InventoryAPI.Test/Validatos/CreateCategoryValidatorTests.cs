using FluentAssertions;
using InventoryAPI.Application.Categories.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Test.Validatos
{
    public class CreateCategoryValidatorTests
    {
        private readonly CreateCategoryValidator _validator = new();

        [Fact]
        public void Validate_ShouldPass_WhenCommandIsValid()
        {
            var command = new CreateCategoryCommand("Electronics", "Electronic products");

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_ShouldFail_WhenNameIsEmpty()
        {
            var command = new CreateCategoryCommand("", "Electronic products");

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
        }

        [Fact]
        public void Validate_ShouldFail_WhenNameExceedsMaxLength()
        {
            var command = new CreateCategoryCommand(new string('A', 101), "Description");

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
        }
    }
}
