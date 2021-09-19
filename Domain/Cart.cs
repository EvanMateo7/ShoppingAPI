using ShoppingAPI.Domain.Interfaces;

namespace ShoppingAPI.Domain
{
  public class Cart : IDomainEntity
    {
        public int Id { get; private set; }

        public string UserId { get; private set; }
        public AppUser User { get; private set; }

        public int ProductId { get; private set; }
        public Product Product { get; private set; }
    }
}
