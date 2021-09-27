using ShoppingAPI.Domain.AggregateRoots.ProductAggregate;
using ShoppingAPI.Domain.Exceptions;

namespace ShoppingAPI.Domain.AggregateRoots.OrderAggregate
{
  public class OrderProduct : IDomainEntity
  {
    public int Id { get; private set; }

    public int OrderId { get; private set; }
    public Order Order { get; private set; }

    public int ProductId { get; private set; }
    public Product Product { get; private set; }

    public decimal Price { get; private set; }

    private float _quantity;
    public float Quantity
    {
      get => _quantity;
      set
      {
        if (value <= 0)
        {
          throw new DomainException(DomainExceptionTypes.OrderProductInvalidQuantity);
        }

        _quantity = value;
      }
    }

    public OrderProduct()
    {
    }

    public OrderProduct(int orderId, Product product, float quantity)
    {
      OrderId = orderId;
      ProductId = product.Id;
      Price = product.Price;
      Quantity = quantity;
    }
  }
}
