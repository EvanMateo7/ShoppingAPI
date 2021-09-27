using ShoppingAPI.Domain.Exceptions;
using ShoppingAPI.Domain.AggregateRoots.ProductAggregate;

namespace ShoppingAPI.Domain.AggregateRoots.AppUserAggregate
{
  public class Cart : IDomainEntity
  {
    public int Id { get; private set; }

    public string UserId { get; private set; }
    public AppUser User { get; private set; }

    public int ProductId { get; private set; }
    public Product Product { get; private set; }

    private float _quantity;
    public float Quantity
    {
      get => _quantity;
      set
      {
        if (value <= 0)
        {
          throw new DomainException(DomainExceptionTypes.CartProductInvalidQuantity);
        }

        _quantity = value;
      }
    }

    public Cart()
    {
    }

    public Cart(string userId, int productId, float quantity)
    {
      UserId = userId;
      ProductId = productId;
      Quantity = quantity;
    }
  }
}
