using System;
using System.Collections.Generic;
using ShoppingAPI.Data.Mappings;
using ShoppingAPI.Domain;

namespace ShoppingAPI.Domain.Repository
{
  public interface IOrderRepository : IRepositoryBase<Order>
  {
    Order Create(IEnumerable<Guid> productIds);

    Order AddProduct(Guid orderId, Guid productId, float quantity);
  }
}
