using System;

namespace ShoppingAPI.API.Controllers.Mappings
{
  public class ProductReadDTO
  {
    public Guid ProductId { get; set; }

    public AppUserReadDTO User { get; private set; }

    public string Name { get; private set; }

#nullable enable
    public string? Description { get; private set; }
#nullable disable

    public float Quantity { get; private set; }

    public decimal Price { get; private set; }

    public DateTime CreatedAt { get; private set; }
  }
}
