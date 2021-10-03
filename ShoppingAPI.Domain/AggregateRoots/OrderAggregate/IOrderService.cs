using System;
using System.Collections.Generic;
using ShoppingAPI.Domain.AggregateRoots.AppUserAggregate;
using ShoppingAPI.Domain.ValueObjects;

namespace ShoppingAPI.Domain.AggregateRoots.OrderAggregate
{
  public interface IOrderService : IServiceBase<Order>
  {
    Order CreateOrder(string userId, IEnumerable<ProductQuantity> productQuantities);

    Order AddRemoveProductsInOrder(Guid orderId, IEnumerable<ProductQuantity> productQuantities);
  }
}
