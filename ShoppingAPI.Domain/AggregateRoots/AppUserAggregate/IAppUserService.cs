using System;
using System.Collections.Generic;
using ShoppingAPI.Domain.AggregateRoots.OrderAggregate;

namespace ShoppingAPI.Domain.AggregateRoots.AppUserAggregate
{
  public interface IAppUserService : IServiceBase<AppUser>
  {
    Order CheckoutCart(string userId);

    void ClearCart(string userId);

    IEnumerable<Cart> AddRemoveProductInCart(string userId, Guid productId, float quantity);
  }
}
