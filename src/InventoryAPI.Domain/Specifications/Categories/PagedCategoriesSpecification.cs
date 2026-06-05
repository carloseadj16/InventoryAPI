using InventoryAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Domain.Specifications.Categories
{
    public class PagedCategoriesSpecification : BaseSpecification<Category>
    {
        public PagedCategoriesSpecification(int page, int pageSize)
        {
            AddCriteria(c => c.Active == 1);
            ApplyOrderBy(c => c.Name);
            ApplyPaging(page, pageSize);
        }
    }
}
