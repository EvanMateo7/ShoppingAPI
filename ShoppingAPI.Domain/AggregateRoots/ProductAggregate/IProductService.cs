using System.Collections.Generic;
using System.Linq;
using ShoppingAPI.Domain.ValueObjects;

namespace ShoppingAPI.Domain.AggregateRoots.ProductAggregate
{
  public interface IProductService : IServiceBase<Product>
  {
    IQueryable<Product> QueryAll(SearchPaginationQuery pageQuery);

    void AllocateProducts(IEnumerable<ProductQuantity> productQuantities);

    void DeallocateProducts(IEnumerable<ProductQuantity> productQuantities);
  }
}
