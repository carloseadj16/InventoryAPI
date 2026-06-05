using FluentAssertions;
using InventoryAPI.Domain.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Test.Specifications
{
    public class PagedProductsSpecificationTests
    {
        [Fact]
        public void Specification_ShouldApplyPaging_WhenPageAndPageSizeProvided()
        {
            var specification = new PagedProductsSpecification(2, 10);

            specification.Skip.Should().Be(10);
            specification.Take.Should().Be(10);
        }

        [Fact]
        public void Specification_ShouldFilterByCategory_WhenCategoryIdProvided()
        {
            var specification = new PagedProductsSpecification(1, 20, categoryId: 3);

            var compiled = specification.Criteria.Compile();
            var products = ProductTestData.GetSampleProducts();

            var result = products.Where(compiled).ToList();

            result.Should().OnlyContain(p => p.CategoryId == 3);
        }

        [Fact]
        public void Specification_ShouldReturnAll_WhenNoCategoryIdProvided()
        {
            var specification = new PagedProductsSpecification(1, 20);

            var compiled = specification.Criteria.Compile();
            var products = ProductTestData.GetSampleProducts();

            var result = products.Where(compiled).ToList();

            result.Should().HaveCount(products.Count);
        }
    }
}
