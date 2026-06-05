using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Application.Movs.Commands
{
    public class RegisterMovValidator : AbstractValidator<RegisterMovCommand>
    {
        public RegisterMovValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("A valid ProductId is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x.MovementType)
                .IsInEnum().WithMessage("MovementType must be In (1) or Out (2).");
        }
    }
}
