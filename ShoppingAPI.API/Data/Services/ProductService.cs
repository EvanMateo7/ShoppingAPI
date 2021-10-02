using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShoppingAPI.Domain.AggregateRoots.ProductAggregate;
using ShoppingAPI.Domain.ValueObjects;

namespace ShoppingAPI.API.Data.Services
{
  public class ProductService : ServiceBase<Product>, IProductService
  {
    private readonly ApplicationContext _appContext;

    public ProductService(ApplicationContext appContext) : base(appContext)
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
