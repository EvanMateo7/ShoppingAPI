using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingAPI.Domain.AggregateRoots.AppUserAggregate;
using ShoppingAPI.Domain.AggregateRoots.OrderAggregate;
using ShoppingAPI.Domain.AggregateRoots.ProductAggregate;

namespace ShoppingAPI.API.Data
{
  public class ApplicationContext : IdentityDbContext<AppUser>
  {
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }
    public DbSet<Product> Products { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderProduct> OrderProducts { get; set; }

    public DbSet<Cart> Cart { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      // Must call IdentityDbContext.OnModelCreating because it has its own implementation
      base.OnModelCreating(builder);

      builder.ApplyConfiguration(new OrderEntityTypeConfiguration());

      builder.ApplyConfiguration(new ProductEntityTypeConfiguration());

      builder.ApplyConfiguration(new OrderProductEntityTypeConfiguration());

      builder.ApplyConfiguration(new CartEntityTypeConfiguration());
    }
  }
}
