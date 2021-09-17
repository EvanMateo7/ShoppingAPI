using System.Linq;
using ShoppingAPI.Data.Util;
using ShoppingAPI.Domain;

namespace ShoppingAPI.Domain.Repository
{
  public interface ICartRepository : IRepositoryBase<Product>
  {
  }
}
