using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingAPI.Domain.AggregateRoots.OrderAggregate;

namespace ShoppingAPI.Database.Data
{
  public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
  {
    public void Configure(EntityTypeBuilder<Order> builder)
    {
      builder.Property(p => p.OrderId).HasDefaultValueSql("newid()");

      builder.HasOne(d => d.User)
              .WithMany(p => p.Orders)
              .HasForeignKey(d => d.UserId);

      builder.HasIndex(e => e.UserId, "IX_Orders_UserId");

      builder.HasIndex(p => p.OrderId).IsUnique();
    }
  }
}
