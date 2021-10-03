using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShoppingAPI.API.Data.Services.Exceptions;
using ShoppingAPI.Domain.AggregateRoots.AppUserAggregate;
using ShoppingAPI.Domain.AggregateRoots.OrderAggregate;
using ShoppingAPI.Domain.AggregateRoots.ProductAggregate;
using ShoppingAPI.Domain.Exceptions;
using ShoppingAPI.Domain.ValueObjects;

namespace ShoppingAPI.API.Data.Services
{
  public class OrderService : ServiceBase<Order>, IOrderService
  {
    private readonly ApplicationContext _appContext;

    public OrderService(ApplicationContext appContext) : base(appContext)
    {
      _appContext = appContext;
    }

    public Order CreateOrder(string userId, IEnumerable<ProductQuantity> productQuantities)
    {
      var newOrder = new Order() { UserId = userId };
      _appContext.Orders.Add(newOrder);
      newOrder = AddRemoveProductInOrder(newOrder.OrderId, productQuantities);
      
      return newOrder;
    }

    public Order AddRemoveProductInOrder(Guid orderId, IEnumerable<ProductQuantity> productQuantities)
    {
      // Find existing or newly added order
      var order = _appContext.Orders
                .Where(o => o.OrderId == orderId)
                .Include(o => o.OrderProducts)
                .FirstOrDefault() ??
                _appContext.Orders.Local.Where(o => o.OrderId == orderId).AsQueryable()
                .Include(o => o.OrderProducts)
                .FirstOrDefault();
      if (order == null)
      {
        throw new DoesNotExist<Order>(new List<Guid> { orderId });
      }

      // Add/Remove order products or update existing ones
      var productIds = productQuantities.Select(pq => pq.ProductId);
      var products = _appContext.Products.Where(p => productIds.Contains(p.ProductId));

      foreach (var product in products)
      {
        var productsNotEnoughStock = new List<ProductQuantity>();
        var quantity = productQuantities.Where(pq => pq.ProductId == product.ProductId).Select(pq => pq.Quantity).Single();

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
      }

      // Save
      _appContext.SaveChanges();
      return order;
    }
  }
}
