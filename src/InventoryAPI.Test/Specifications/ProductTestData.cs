using InventoryAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Test.Specifications
{
    public static class ProductTestData
    {
        public static List<Product> GetSampleProducts()
        {
            return new List<Product>
        {
            Product.Create("Product A", "Desc", 10m, 5, 1),
            Product.Create("Product B", "Desc", 20m, 3, 1),
            Product.Create("Product C", "Desc", 30m, 8, 2),
            Product.Create("Product D", "Desc", 15m, 2, 3)
        };
        }
    }
}
