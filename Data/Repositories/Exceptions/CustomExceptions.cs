

using System;
using System.Collections.Generic;

namespace ShoppingAPI.Data.Repositories.Exceptions
{
  public class ProductDoesNotExist : Exception
  {
    public IEnumerable<Guid> ProductIds { get; init; }
    
    public ProductDoesNotExist(IEnumerable<Guid> productIds)
      : base($"Following products do no exist: {String.Join(", ", productIds)}")
    {
      ProductIds = productIds;
    }
  }
}
