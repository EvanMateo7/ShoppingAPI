using System.Linq;
using ShoppingAPI.Domain.ValueObjects;

namespace ShoppingAPI.Domain.AggregateRoots.ProductAggregate
{
  public interface IProductService : IServiceBase<Product>
  {
    IQueryable<Product> QueryAll(SearchPaginationQuery pageQuery);
  }
}
