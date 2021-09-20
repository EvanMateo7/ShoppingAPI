

using System;
using System.Linq;
using System.Collections.Generic;
using ShoppingAPI.Domain.Interfaces;

namespace ShoppingAPI.Data.Repositories.Exceptions
{
  public class DoesNotExist : Exception
  {
    public IEnumerable<dynamic> Ids { get; init; }
    
    public DoesNotExist(IEnumerable<dynamic> ids, string msg) : base(msg)
    {
      Ids = ids;
    }

    public DoesNotExist(IEnumerable<Guid> ids, string msg) : base(msg)
    {
      Ids = ids.Select(id => id.ToString());
    }
  }

  public class DoesNotExist<T> : DoesNotExist where T : IDomainEntity
  {
    public static string message = $"One or more {typeof(T).Name.ToLower()}(s) do not exist";

    public DoesNotExist(IEnumerable<dynamic> ids)
      : base(ids, message)
    {
    }

    public DoesNotExist(IEnumerable<Guid> ids)
      : base(ids, message)
    {
    }
  }
  
  public class NotEnoughProductInStock : Exception
  {
    public float Quantity { get; init; }
    public NotEnoughProductInStock(float quantity) : base("Not enough product in stock")
    {
      Quantity = quantity;
    }
  }
}
