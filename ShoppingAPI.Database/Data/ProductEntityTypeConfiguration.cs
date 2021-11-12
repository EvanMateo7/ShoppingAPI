using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingAPI.Domain.AggregateRoots.ProductAggregate;

namespace ShoppingAPI.Database.Data
{
  public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
  {
    public void Configure(EntityTypeBuilder<Product> builder)
    {
      builder.Property(e => e.Name).IsRequired();

      builder.Property(p => p.ProductId).HasDefaultValueSql("newid()");

      builder.Property(e => e.Name).HasMaxLength(69);

      builder.Property(e => e.Price).HasColumnType("decimal(18, 2)");

      builder.Property(p => p.RowVersion).IsRowVersion();

      builder.HasOne(d => d.User)
          .WithMany(p => p.Products)
          .HasForeignKey(d => d.UserId);

      builder.HasIndex(e => e.UserId, "IX_Products_UserId");

      builder.HasIndex(p => p.ProductId).IsUnique();
    }
  }
}
