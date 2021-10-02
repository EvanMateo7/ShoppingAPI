using System;
using System.Collections.Generic;
using ShoppingAPI.Domain.AggregateRoots.AppUserAggregate;
using ShoppingAPI.Domain.ValueObjects;

namespace ShoppingAPI.Domain.AggregateRoots.OrderAggregate
{
  public interface IOrderService : IServiceBase<Order>
  {
    Order CreateOrder(AppUser user);

    Order AddRemoveProductInOrder(Guid orderId, IEnumerable<ProductQuantity> productQuantities);
  }
}
