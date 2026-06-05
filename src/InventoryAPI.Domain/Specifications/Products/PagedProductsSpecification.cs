using InventoryAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Domain.Specifications.Products
{
    public class PagedProductsSpecification : BaseSpecification<Product>
    {
        public PagedProductsSpecification(int page, int pageSize, int? categoryId = null)
        {
            AddCriteria(p => p.Active == 1 && (categoryId == null || p.CategoryId == categoryId));
            ApplyOrderBy(p => p.Name);
            ApplyPaging(page, pageSize);
        }
    }
}
