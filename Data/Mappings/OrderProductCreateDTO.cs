using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ShoppingAPI.Domain;

namespace ShoppingAPI.Data.Mappings
{
  public class OrderProductCreateDTO
  {
    [Required]
    public Guid ProductId { get; set; }

    public float Quantity { get; set; }
  }
}
