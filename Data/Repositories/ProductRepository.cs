using ShoppingAPI.Domain;
using ShoppingAPI.Domain.Repository;

namespace ShoppingAPI.Data.Repositories
{
  public class ProductRepository : RepositoryBase<Product>, IProductRepository
  {
    public ProductRepository(ApplicationContext appContext) : base(appContext)
    {
    }
  }
}
