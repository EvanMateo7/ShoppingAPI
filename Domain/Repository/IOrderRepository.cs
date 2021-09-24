using System;
using System.Collections.Generic;
using ShoppingAPI.Data.Repositories.Records;
using ShoppingAPI.Domain;

namespace ShoppingAPI.Domain.Repository
{
  public interface IOrderRepository : IRepositoryBase<Order>
  {
    Order Create(AppUser user);

    Order AddRemoveProductInOrder(Guid orderId, IEnumerable<ProductQuantity> productQuantities);
  }
}
