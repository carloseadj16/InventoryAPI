using FluentAssertions;
using InventoryAPI.Application.Movs.Commands;
using InventoryAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Test.Validatos
{
    public class RegisterMovementValidatorTests
    {
        private readonly RegisterMovValidator _validator = new();

        [Fact]
        public void Validate_ShouldPass_WhenCommandIsValid()
        {
            var command = new RegisterMovCommand("CCeeFF12",1, 5, MovementType.In, "Purchase");

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_ShouldFail_WhenQuantityIsZero()
        {
            var command = new RegisterMovCommand("AbCD12A",1, 0, MovementType.In, "Purchase");

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Quantity");
        }

        [Fact]
        public void Validate_ShouldFail_WhenProductIdIsZero()
        {
            var command = new RegisterMovCommand("CCeeFF12", 0, 5, MovementType.In, "Purchase");

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "ProductId");
        }

        [Fact]
        public void Validate_ShouldFail_WhenMovementTypeIsInvalid()
        {
            var command = new RegisterMovCommand("ffA12Fg",1, 5, (MovementType)99, "Purchase");

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "MovementType");
        }
    }
}
