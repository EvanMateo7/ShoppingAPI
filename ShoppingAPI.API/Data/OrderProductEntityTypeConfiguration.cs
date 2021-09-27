using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingAPI.Domain.AggregateRoots.OrderAggregate;

namespace ShoppingAPI.API.Data
{
  public class OrderProductEntityTypeConfiguration : IEntityTypeConfiguration<OrderProduct>
  {
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
      builder.Property(e => e.Price).HasColumnType("decimal(18, 2)");

      builder.HasOne(d => d.Order)
          .WithMany(p => p.OrderProducts)
          .HasForeignKey(d => d.OrderId);

      builder.HasOne(d => d.Product)
          .WithMany(p => p.OrderProducts)
          .HasForeignKey(d => d.ProductId);

      builder.HasIndex(e => e.OrderId, "IX_OrderProducts_OrderId");

      builder.HasIndex(e => e.ProductId, "IX_OrderProducts_ProductId");
    }
  }
}
