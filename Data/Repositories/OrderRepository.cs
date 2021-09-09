using System;
using System.Collections.Generic;
using System.Linq;
using ShoppingAPI.Data.Mappings;
using ShoppingAPI.Data.Repositories.Exceptions;
using ShoppingAPI.Domain;
using ShoppingAPI.Domain.Repository;

namespace ShoppingAPI.Data.Repositories
{
  public class OrderRepository : RepositoryBase<Order>, IOrderRepository
  {
    private readonly ApplicationContext _appContext;

    public OrderRepository(ApplicationContext appContext) : base(appContext)
    {
      _appContext = appContext;
    }

    public Order Create(IEnumerable<Guid> productIds)
    {
      var products = _appContext.Products
                      .Where(p => productIds.Contains(p.ProductId));
                    
      var ids = products.Select(p => p.ProductId);

      var nonExistingProducts = productIds.Except(ids);
      if (nonExistingProducts.Count() > 0)
      {
        throw new ProductDoesNotExist(nonExistingProducts);
      }

      using var transaction = _appContext.Database.BeginTransaction();
      try
      {
        // Create order
        var newOrder = new Order();
        _appContext.Orders.Add(newOrder);
        _appContext.SaveChanges();

        // Add products to order
        var orderProducts = new List<OrderProduct>();
        foreach (var product in products)
        {
          var newOrderProduct = new OrderProduct(newOrder.Id, product);
          orderProducts.Add(newOrderProduct);
        }
        _appContext.OrderProducts.AddRange(orderProducts);
        _appContext.SaveChanges();
  
        // Save
        transaction.Commit();
        return newOrder;
      }
      catch (System.Exception)
      {
        throw new Exception();
      }
    }
  }
}
