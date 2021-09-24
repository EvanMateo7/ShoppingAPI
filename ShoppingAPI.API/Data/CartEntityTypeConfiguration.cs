using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAPI.Data
{
    public class CartEntityTypeConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasOne(c => c.User)
                .WithMany(u => u.CartProducts)
                .HasForeignKey(c => c.UserId);

            builder.HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(d => d.ProductId);

            builder.HasIndex(e => e.UserId, "IX_Cartasss_UserId");

            builder.HasIndex(e => e.ProductId, "IX_Cart_ProductId");
        }
    }
}
