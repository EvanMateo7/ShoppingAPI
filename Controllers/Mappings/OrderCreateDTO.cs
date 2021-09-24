using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ShoppingAPI.Domain;

namespace ShoppingAPI.Controllers.Mappings
{
  public class OrderCreateDTO
  {
    [Required]
    public IEnumerable<Guid> ProductIDs { get; set; }
  }
}