using System;
using System.ComponentModel.DataAnnotations;

namespace ShoppingAPI.API.Controllers.Mappings
{
  public class OrderProductCreateDTO
  {
    [Required]
    public Guid ProductId { get; set; }

    public float Quantity { get; set; }
  }
}
