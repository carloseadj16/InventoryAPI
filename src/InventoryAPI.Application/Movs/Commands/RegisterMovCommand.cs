using InventoryAPI.Application.Common;
using InventoryAPI.Domain.Entities;
using InventoryAPI.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Application.Movs.Commands
{
    public record RegisterMovCommand(
        string RequestId,
        int ProductId,
        int Quantity,
        MovementType MovementType,
        string Reason
    ) : IRequest<Result<int>>, IIdempotentCommand;

}
