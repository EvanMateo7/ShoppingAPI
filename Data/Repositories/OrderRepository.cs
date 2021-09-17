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

    public Order AddRemoveProduct(Guid orderId, Guid productId, float quantity)
    {
      bool saveFailed;
      do
      {
        saveFailed = false;
        try
        {
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

          // Validate product quantity change
          try
          {
            product.Quantity -= quantity;
          }
          catch (DomainException e)
          {
            if (e.DomainExceptionType == DomainExceptionTypes.ProductNegativeQuantity)
            {
              throw new NotEnoughProductInStock(quantity);
            }
          }

          // Add/Remove order product or update existing one
          var orderProduct = _appContext.OrderProducts
                              .Where(op => op.OrderId == order.Id && op.ProductId == product.Id)
                              .FirstOrDefault();

          if (orderProduct != null)
          {
            try
            {
              orderProduct.Quantity += quantity;
            }
            catch (DomainException e)
            {
              if (e.DomainExceptionType == DomainExceptionTypes.OrderProductInvalidQuantity)
              {
                _appContext.Remove(orderProduct);
              }
            }
          }
          else
          {
            if (quantity > 0)
            {
              var newOrderProduct = new OrderProduct(order.Id, product, quantity);
              order.OrderProducts.Add(newOrderProduct);
            }
          }

          // Save
          _appContext.SaveChanges();
          return order;
        }
        catch (DbUpdateConcurrencyException e)
        {
          saveFailed = true;

          // Reload product entity
          e.Entries.Single().Reload();
        }
      } while (saveFailed);

      return null;
    }
  }
}
