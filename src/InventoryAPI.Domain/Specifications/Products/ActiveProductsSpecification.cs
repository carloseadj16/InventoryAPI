using InventoryAPI.Domain.Entities;


namespace InventoryAPI.Domain.Specifications.Products
{
    public class ActiveProductsSpecification : BaseSpecification<Product>
    {
        public ActiveProductsSpecification()
        {
            AddCriteria(p => p.Active == 1);
            ApplyOrderBy(p => p.Name);
        }
    }
}
