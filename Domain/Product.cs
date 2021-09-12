using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ShoppingAPI.Domain.Exceptions;

namespace ShoppingAPI.Domain
{
    public class Product : IDomainEntity
    {
        public int Id { get; private set; }

        public Guid ProductId { get; private set; }

        public string UserId { get; private set; }
        public AppUser User { get; private set; }

        public string Name { get; private set; }

#nullable enable
        public string? Description { get; private set; }
#nullable disable

        private float _quantity;
        public float Quantity
        {
          get => _quantity;
          set {
            if (value < 0)
            {
              throw new DomainException(DomainExceptionTypes.ProductNegativeQuantity);
            }
            _quantity = value;
          } 
        }

        public decimal Price { get; private set; }

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public ICollection<OrderProduct> OrderProducts { get; private set; }
    }
}
