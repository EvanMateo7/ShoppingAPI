using System;
using System.ComponentModel.DataAnnotations;

namespace ShoppingAPI.Data.Mappings
{
  public class ProductCreateDTO
  {
    [Required]
    public string UserId { get; set; }

    [Required]
    public string Name { get; set; }

#nullable enable
    public string? Description { get; set; }
#nullable disable

    [Range(0, Int32.MaxValue, ErrorMessage = "quantity should not be less than zero.")]
    public float Quantity { get; set; }

    [Range(0, Int32.MaxValue, ErrorMessage = "price should not be less than zero.")]
    public decimal Price { get; set; }
  }
}
