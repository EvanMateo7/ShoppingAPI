using System;
using System.Collections.Generic;
using System.Linq;
using ShoppingAPI.Data.Util;
using ShoppingAPI.Domain;

namespace ShoppingAPI.Domain.Repository
{
  public interface IAppUserRepository : IRepositoryBase<AppUser>
  {
    IEnumerable<Cart> AddRemoveProductInCart(string userId, Guid productId, float quantity);
  }
}
