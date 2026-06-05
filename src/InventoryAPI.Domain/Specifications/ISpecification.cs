using System.Linq.Expressions;

namespace InventoryAPI.Domain.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        Expression<Func<T, object>>? OrderBy { get; }
        int? Take { get; }
        int? Skip { get; }
    }
}
