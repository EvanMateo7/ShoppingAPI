using System.Linq;
using ShoppingAPI.Domain.ValueObjects;

namespace ShoppingAPI.Domain.AggregateRoots.ProductAggregate
{
  public interface IProductRepository : IRepositoryBase<Product>
  {
    IQueryable<Product> QueryAll(SearchPaginationQuery pageQuery);
  }
}
