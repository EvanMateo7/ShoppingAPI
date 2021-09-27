using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using ShoppingAPI.Domain.AggregateRoots.OrderAggregate;
using ShoppingAPI.Domain.AggregateRoots.ProductAggregate;

namespace ShoppingAPI.Domain.AggregateRoots.AppUserAggregate
{
  public class AppUser : IdentityUser, IDomainEntity, IAggregateRoot
  {
    public string First_Name { get; private set; }

    public string Last_Name { get; private set; }

    public ICollection<Order> Orders { get; private set; } = new HashSet<Order>();

    public ICollection<Product> Products { get; private set; } = new HashSet<Product>();

    public ICollection<Cart> CartProducts { get; private set; } = new HashSet<Cart>();
  }
}
