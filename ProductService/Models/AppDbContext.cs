using Microsoft.EntityFrameworkCore;

namespace ProductService.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<Unit> Units { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity
                    .HasOne(c => c.User)
                    .WithMany(u => u.Commnents)
                    .HasForeignKey(c => c.UserId);

                entity.HasOne(c => c.Product)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(c => c.ProductId);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

                entity.HasOne(p => p.Unit_)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.UnitId)
                .IsRequired(false); ;
            });

            modelBuilder.Entity<ProductDetail>(entity =>
            {
                entity.HasOne(pd => pd.Product)
                .WithMany(p => p.ProductDetails)
                .HasForeignKey(pd => pd.ProductId);
            });

            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedNever();
        }
    }
}
