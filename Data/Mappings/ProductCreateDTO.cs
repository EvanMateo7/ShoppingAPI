using System;

namespace ShoppingAPI.Data.Mappings
{
  public class ProductCreateDTO
    {
        public string UserId { get; set; }

        public string Name { get; set; }

#nullable enable
        public string? Description { get; set; }
#nullable disable

        public float Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
