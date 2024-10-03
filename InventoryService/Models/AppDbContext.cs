using Microsoft.EntityFrameworkCore;

namespace InventoryService.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Import> Imports { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ImportDetail> ImportDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
