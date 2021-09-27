using System;
using System.Collections.Generic;
using ShoppingAPI.Domain.AggregateRoots.AppUserAggregate;
using ShoppingAPI.Domain.ValueObjects;

namespace ShoppingAPI.Domain.AggregateRoots.OrderAggregate
{
  public interface IOrderRepository : IRepositoryBase<Order>
  {
    Order Create(AppUser user);

    Order AddRemoveProductInOrder(Guid orderId, IEnumerable<ProductQuantity> productQuantities);
  }
}
