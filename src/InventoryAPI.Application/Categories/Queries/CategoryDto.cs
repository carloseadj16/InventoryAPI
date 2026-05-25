using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Application.Categories.Queries
{
    public record CategoryDto(
        int Id,
        string Name,
        string Description,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}
