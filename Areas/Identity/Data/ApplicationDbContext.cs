using garage.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace garage.Areas.Identity.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Cart>(entity =>
            {
                entity.ToTable("Cart");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserName).HasMaxLength(256); // Add this line to configure the UserName property
                entity.HasOne(e => e.Product)
                      .WithMany(p => p.Carts)
                      .HasForeignKey(e => e.ProductId);
            });

            builder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
                entity.HasKey(e => e.Id);
            });

            builder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Cat)
                      .WithMany(c => c.Products)
                      .HasForeignKey(e => e.Catid);
                entity.Property(e => e.Price)
                      .HasColumnType("decimal(18,2)"); // Specify precision and scale
            });

            builder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contacts");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Message).IsRequired().HasMaxLength(500);
            });
        }

        public void SeedData()
        {
            if (!Categories.Any())
            {
                Categories.AddRange(
                    new Category { Id = 1, Name = "Electronics", Description = "Electronic items" },
                    new Category { Id = 2, Name = "Books", Description = "Books and literature" },
                    new Category { Id = 3, Name = "Clothing", Description = "Apparel and accessories" }
                );
            }

            if (!Products.Any())
            {
                Products.AddRange(
                    new Product { Id = 1, Name = "Laptop", Description = "A high-performance laptop", Price = 999.99m, Catid = 1, Photo = "laptop.jpg" },
                    new Product { Id = 2, Name = "Smartphone", Description = "A latest model smartphone", Price = 699.99m, Catid = 1, Photo = "smartphone.jpg" },
                    new Product { Id = 3, Name = "Novel", Description = "A best-selling novel", Price = 19.99m, Catid = 2, Photo = "novel.jpg" },
                    new Product { Id = 4, Name = "T-Shirt", Description = "A comfortable t-shirt", Price = 9.99m, Catid = 3, Photo = "tshirt.jpg" }
                );
            }

            SaveChanges();
        }
    }
}
