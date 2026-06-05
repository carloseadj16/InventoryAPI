using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Application.Common
{
    public interface IIdempotentCommand
    {
        string RequestId { get; }
    }
}
