
using System;
using System.ComponentModel.DataAnnotations;

namespace ShoppingAPI.Domain.ValueObjects
{
  public record ProductQuantity([Required] Guid ProductId, float Quantity);
}
