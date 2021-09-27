using System;

namespace ShoppingAPI.API.Controllers.Mappings
{
  public class OrderReadDTO
    {
        public int Id { get; private set; }

        public Guid OrderId { get; private set; }

        public AppUserReadDTO User { get; private set; }

#nullable enable
        public DateTime? FullfilledAt { get; private set; }
#nullable disable

        public DateTime CreatedAt { get; private set; }
    }
}
