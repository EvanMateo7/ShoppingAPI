using System;
using System.Collections.Generic;
using ShoppingAPI.Domain.AggregateRoots.AppUserAggregate;
using ShoppingAPI.Domain.AggregateRoots.OrderAggregate;
using ShoppingAPI.Domain.Exceptions;

namespace ShoppingAPI.Domain.AggregateRoots.ProductAggregate
{
  public class Product : IDomainEntity, IAggregateRoot
  {
    public int Id { get; set; }

    public Guid ProductId { get; set; }

    public string UserId { get; set; }
    public AppUser User { get; set; }

    public string Name { get; set; }

#nullable enable
    public string? Description { get; set; }
#nullable disable

    private float _quantity;
    public float Quantity
    {
      get => _quantity;
      set
      {
        if (value < 0)
        {
          throw new DomainException(DomainExceptionTypes.ProductNegativeQuantity);
        }
        _quantity = value;
      }
    }

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public byte[] Timestamp { get; set; }

    public ICollection<OrderProduct> OrderProducts { get; set; } = new HashSet<OrderProduct>();
  }
}
