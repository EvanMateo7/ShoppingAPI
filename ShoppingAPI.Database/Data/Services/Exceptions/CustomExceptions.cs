

using System;
using System.Linq;
using System.Collections.Generic;
using ShoppingAPI.Domain.AggregateRoots;
using ShoppingAPI.Domain.ValueObjects;

namespace ShoppingAPI.Database.Data.Services.Exceptions
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

  public class NotEnoughProductsInStock : Exception
  {
    public static string message = "Not enough products in stock";

    public IEnumerable<ProductQuantity> productQuantities { get; init; }

    public NotEnoughProductsInStock(IEnumerable<ProductQuantity> data) : base(message)
    {
      productQuantities = data;
    }

    public NotEnoughProductsInStock(ProductQuantity data) : base(message)
    {
      productQuantities = new List<ProductQuantity> { data };
    }
  }

  public class EmptyCart : Exception
  {
    public EmptyCart() : base("Cart is empty")
    {
    }
  }
}
