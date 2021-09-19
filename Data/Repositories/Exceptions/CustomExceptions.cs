

using System;
using System.Linq;
using System.Collections.Generic;
using ShoppingAPI.Domain.Interfaces;

namespace ShoppingAPI.Data.Repositories.Exceptions
{
  public class DoesNotExistBase : Exception
  {
    public IEnumerable<dynamic> Ids { get; init; }
    
    public DoesNotExistBase(IEnumerable<dynamic> ids, string msg) : base(msg)
    {
      Ids = ids;
    }

    public DoesNotExistBase(IEnumerable<Guid> ids, string msg) : base(msg)
    {
      Ids = ids.Select(id => id.ToString());
    }
  }

  public class DoesNotExist<T> : DoesNotExistBase where T : IDomainEntity
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
