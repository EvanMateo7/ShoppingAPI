
using System;
using System.ComponentModel.DataAnnotations;

namespace ShoppingAPI.Data.Repositories.Records
{
    public record ProductQuantity( [Required] Guid ProductId, float Quantity);
}
