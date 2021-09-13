using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShoppingAPI.Data.Mappings;
using ShoppingAPI.Data.Repositories.Exceptions;
using ShoppingAPI.Domain;
using ShoppingAPI.Domain.Exceptions;
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
        throw new DoesNotExist<Product>(nonExistingProducts);
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
          // TODO: Validate quantity or remove this repo method
          var newOrderProduct = new OrderProduct(newOrder.Id, product, product.Quantity);
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

    public Order AddProduct(Guid orderId, Guid productId, float quantity)
    {
      return _appContext.Database.CreateExecutionStrategy().Execute(() =>
      {
        using var transaction = _appContext.Database.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);
        try
        {
          // Ensures fresh data is fetched and not data changed from previous transaction.
          // This is similar to creating a new context.
          // https://github.com/dotnet/efcore/issues/23850#issuecomment-809744519
          _appContext.ChangeTracker.Clear(); 

          var order = _appContext.Orders
                      .Where(o => o.OrderId == orderId)
                      .Include(o => o.OrderProducts)
                      .FirstOrDefault();

          var product = _appContext.Products
                          .Where(p => p.ProductId == productId)
                          .FirstOrDefault();

          if (order == null)
          {
            throw new DoesNotExist<Order>(new List<Guid> { orderId });
          }
          if (product == null)
          {
            throw new DoesNotExist<Product>(new List<Guid> { productId });
          }

          // Validate quantity
          try
          {
            if (quantity < 1)
            {
              throw new InvalidProductQuantity(quantity); 
            }
            product.Quantity -= quantity;   
          }
          catch (DomainException e)
          {
            if (e.DomainExceptionType == DomainExceptionTypes.ProductNegativeQuantity)
            {
              throw new InvalidProductQuantity(quantity);
            }
          }

          // Add new order product
          var newOrderProduct = new OrderProduct(order.Id, product, quantity);
          order.OrderProducts.Add(newOrderProduct);

          _appContext.SaveChanges();

          // Save
          transaction.Commit();
          return order;
        }
        catch (DbUpdateConcurrencyException e)
        {
          throw e;
        }
      });
    }
  }
}
