using System;
using System.Collections.Generic;

namespace ShoppingAPI.Domain.AggregateRoots.AppUserAggregate
{
  public interface IAppUserRepository : IRepositoryBase<AppUser>
  {
    IEnumerable<Cart> AddRemoveProductInCart(string userId, Guid productId, float quantity);
  }
}
