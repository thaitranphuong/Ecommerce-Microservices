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

        public DbSet<ExportProduct> ExportProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExportProduct>(entity =>
            {
                entity.HasKey(o => new { o.WarehouseId, o.Id });
            });
        }
    }
}
