using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAPI.Domain
{
    public class Product
    {
        public int Id { get; private set; }

        public string UserId { get; private set; }
        public AppUser User { get; private set; }

        public string Name { get; private set; }

#nullable enable
        public string? Description { get; private set; }
#nullable disable

        public float Quantity { get; private set; }

        public decimal Price { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public ICollection<OrderProduct> OrderProducts { get; private set; }
    }
}
