using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ShoppingAPI.Data.Util;
using ShoppingAPI.Domain;
using ShoppingAPI.Domain.Repository;

namespace ShoppingAPI.Data.Repositories
{
  public class ProductRepository : RepositoryBase<Product>, IProductRepository
  {
    private readonly ApplicationContext _appContext;

    public ProductRepository(ApplicationContext appContext) : base(appContext)
    {
      _appContext = appContext;
    }

    public IQueryable<Product> QueryAll(SearchPaginationQuery pageQuery)
    {
      var query = _appContext
                    .Products
                    .Where(p => p.Name.Contains(pageQuery.SearchTerm))
                    .OrderBy(p => p.ProductId)
                    .Skip((pageQuery.PageNumber - 1) * pageQuery.PageSize)
                    .Take(pageQuery.PageSize)
                    .AsNoTracking();

      return query;
    }
  }
}
