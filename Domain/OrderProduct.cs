using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAPI.Domain
{
    public class OrderProduct : IDomainEntity
    {
        public int Id { get; private set; }

        public int OrderId { get; private set; }
        public Order Order { get; private set; }

        public int ProductId { get; private set; }
        public Product Product { get; private set; }

        public decimal Price { get; private set; }

        public float Quantity { get; private set; }

        public OrderProduct()
        {
        }

        public OrderProduct(int orderId, Product product)
        {
          OrderId = orderId;
          ProductId = product.Id;
          Price = product.Price;
          Quantity = product.Quantity;
        }
    }
}
