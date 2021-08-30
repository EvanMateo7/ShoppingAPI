using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAPI.Data
{
    public class ApplicationContext : IdentityDbContext<AppUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderProduct> OrderProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Must call IdentityDbContext.OnModelCreating because it has its own implementation
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new OrderEntityTypeConfiguration());

            builder.ApplyConfiguration(new ProductEntityTypeConfiguration());

            builder.ApplyConfiguration(new OrderProductEntityTypeConfiguration());
        }
    }
}
