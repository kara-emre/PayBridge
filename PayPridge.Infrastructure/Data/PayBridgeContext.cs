using Microsoft.EntityFrameworkCore;
using PayPridge.Domain.Entities;

namespace PayPridge.Infrastructure.Data
{
    public class PayBridgeContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public PayBridgeContext(DbContextOptions<PayBridgeContext> options)
        : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18, 2)");  
        }
    }
}
