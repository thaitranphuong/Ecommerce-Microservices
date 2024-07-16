using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>(entity => {
                entity.HasOne(o => o.Voucher)
                        .WithMany(v => v.Orders)
                        .HasForeignKey(o => o.VoucherId);
            });

            modelBuilder.Entity<OrderDetail>(entity => {
                entity.HasKey(o => new { o.OrderId, o.ProductId });

                entity.HasOne(o => o.Order)
                        .WithMany(v => v.OrderDetails)
                        .HasForeignKey(o => o.OrderId);
            });
        }
    }
}
