using System.Linq;
using ShoppingAPI.Data.Util;
using ShoppingAPI.Domain;

namespace ShoppingAPI.Domain.Repository
{
  public interface IProductRepository : IRepositoryBase<Product>
  {
    IQueryable<Product> QueryAll(SearchPaginationQuery pageQuery);
  }
}
