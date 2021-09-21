using System;
using System.Collections.Generic;
using ShoppingAPI.Data.Mappings;
using ShoppingAPI.Data.Repositories.Records;
using ShoppingAPI.Domain;

namespace ShoppingAPI.Domain.Repository
{
  public interface IOrderRepository : IRepositoryBase<Order>
  {
    Order Create(AppUser user);

    Order AddRemoveProduct(Order order, IEnumerable<ProductQuantity> productQuantities);
  }
}
