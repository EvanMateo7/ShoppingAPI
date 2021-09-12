

using System;
using System.Collections.Generic;
using ShoppingAPI.Domain;

namespace ShoppingAPI.Data.Repositories.Exceptions
{
  public class DoesNotExist : Exception
  {
    public IEnumerable<Guid> Ids { get; init; }
    
    public DoesNotExist(IEnumerable<Guid> ids, string msg) : base(msg)
    {
      Ids = ids;
    }
  }

  public class DoesNotExist<T> : DoesNotExist where T : IDomainEntity
  {
    public DoesNotExist(IEnumerable<Guid> ids)
      : base(ids, $"One or more {typeof(T).Name.ToLower()}(s) do not exist")
    {
    }
  }
  
  public class InvalidProductQuantity : Exception
  {
    public float InvalidQuantity { get; init; }
    public InvalidProductQuantity(float invalidQuantity) : base("Product quantity is either less than one or too large")
    {
      InvalidQuantity = invalidQuantity;
    }
  }
}
