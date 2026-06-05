using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string resource, object id) : base($"{resource} with id '{id}' was not found.") { }
    }
}
