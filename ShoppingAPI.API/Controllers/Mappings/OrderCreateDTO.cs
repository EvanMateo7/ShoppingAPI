using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingAPI.API.Controllers.Mappings
{
  public class OrderCreateDTO
  {
    [Required]
    public IEnumerable<Guid> ProductIDs { get; set; }
  }
}
