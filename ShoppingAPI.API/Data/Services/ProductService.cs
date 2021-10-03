using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShoppingAPI.API.Data.Services.Exceptions;
using ShoppingAPI.Domain.AggregateRoots.ProductAggregate;
using ShoppingAPI.Domain.Exceptions;
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

    public void AllocateProducts(IEnumerable<ProductQuantity> productQuantities)
    {
      UpdateProductQuantities(productQuantities);
    }

    public void DeallocateProducts(IEnumerable<ProductQuantity> productQuantities)
    {
      var negativeProductQuantities = productQuantities.Select(pq =>new ProductQuantity(pq.ProductId, -pq.Quantity));
      UpdateProductQuantities(negativeProductQuantities);
    }

    private void UpdateProductQuantities(IEnumerable<ProductQuantity> productQuantities)
    {
      bool saveFailed;
      do
      {
        saveFailed = false;
        try
        {
          var productIds = productQuantities.Select(pq => pq.ProductId);

          var products = _appContext.Products.Where(p => productIds.Contains(p.ProductId));

          var ids = products.Select(p => p.ProductId);
          var nonExistingProducts = productIds.Except(ids);
          if (nonExistingProducts.Count() > 0)
          {
            throw new DoesNotExist<Product>(nonExistingProducts);
          }

          foreach (var product in products)
          {
            // Validate product quantity change
            var productsNotEnoughStock = new List<ProductQuantity>();
            var quantity = productQuantities.Where(pq => pq.ProductId == product.ProductId).Select(pq => pq.Quantity).Single();

            try
            {
              product.Quantity -= quantity;
            }
            catch (DomainException e)
            {
              if (e.DomainExceptionType == DomainExceptionTypes.ProductNegativeQuantity)
              {
                productsNotEnoughStock.Add(new ProductQuantity(product.ProductId, quantity));
              }
            }

            if (productsNotEnoughStock.Count() > 0)
            {
              throw new NotEnoughProductsInStock(new ProductQuantity(product.ProductId, quantity));
            }
          }

          _appContext.SaveChanges();
        }
        catch (DbUpdateConcurrencyException e)
        {
          saveFailed = true;

          // Reload product entity
          e.Entries.Single().Reload();
        }
      } while (saveFailed);
    }
  }
}
