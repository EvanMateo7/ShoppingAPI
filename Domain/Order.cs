using System;
using System.Collections.Generic;
using ShoppingAPI.Domain.Interfaces;

namespace ShoppingAPI.Domain
{
  public class Order : IDomainEntity, IAggregateRoot
    {
        public int Id { get; private set; }

        public Guid OrderId { get; private set; }

        public string UserId { get; private set; }
        public AppUser User { get; private set; }

#nullable enable
        public DateTime? FullfilledAt { get; private set; }
#nullable disable

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public ICollection<OrderProduct> OrderProducts { get; private set; }
    }
}
