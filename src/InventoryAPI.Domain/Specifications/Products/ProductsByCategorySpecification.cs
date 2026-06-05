using InventoryAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Domain.Specifications.Products
{
    public class ProductsByCategorySpecification : BaseSpecification<Product>
    {
        public ProductsByCategorySpecification(int categoryId)
        {
            AddCriteria(p => p.Active == 1 && p.CategoryId == categoryId);
            ApplyOrderBy(p => p.Name);
        }
    }
}
