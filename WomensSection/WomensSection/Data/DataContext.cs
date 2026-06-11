using Maia.Models;
using Microsoft.EntityFrameworkCore;

namespace Maia.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<CardsWomen> CardsWoman { get; set; }
        public DbSet<WomanCategory> WomanCategories { get; set; }

        // ORDERS
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CardsWomen>()
                .ToTable("CardsWomen");

            modelBuilder.Entity<WomanCategory>()
                .ToTable("WomanCategories");

            modelBuilder.Entity<Order>()
                .ToTable("Orders");

            modelBuilder.Entity<OrderItem>()
                .ToTable("OrderItems");

            modelBuilder.Entity<CardsWomen>()
                .HasOne(c => c.WomanCategory)
                .WithMany()
                .HasForeignKey(c => c.WomanCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Wishlist>()
                .HasMany(w => w.WishlistItems)
                .WithOne(wi => wi.Wishlist)
                .HasForeignKey(wi => wi.WishlistId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WishlistItem>()
                .Property(w => w.Source)
                .HasDefaultValue("WOMAN");

            modelBuilder.Entity<WishlistItem>()
                .Property(w => w.ProductPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CardsWomen>()
                .Property(x => x.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CardsWomen>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Timestamps match what the migrations applied to the DB
            modelBuilder.Entity<WomanCategory>().HasData(
                new WomanCategory { Id = 1, Name = "Tops",          CreatedAt = new DateTime(2026, 6, 4, 20, 35, 10, 790, DateTimeKind.Utc).AddTicks(6826) },
                new WomanCategory { Id = 2, Name = "Dresses",       CreatedAt = new DateTime(2026, 6, 4, 20, 35, 10, 790, DateTimeKind.Utc).AddTicks(7298) },
                new WomanCategory { Id = 3, Name = "Bottoms",       CreatedAt = new DateTime(2026, 6, 4, 20, 35, 10, 790, DateTimeKind.Utc).AddTicks(7299) },
                new WomanCategory { Id = 4, Name = "Outerwear",     CreatedAt = new DateTime(2026, 6, 4, 20, 35, 10, 790, DateTimeKind.Utc).AddTicks(7299) },
                new WomanCategory { Id = 5, Name = "Swimwear",      CreatedAt = new DateTime(2026, 6, 5, 17, 57, 58, 886, DateTimeKind.Utc).AddTicks(6667) },
                new WomanCategory { Id = 6, Name = "Matching Sets", CreatedAt = new DateTime(2026, 6, 5, 17, 57, 58, 886, DateTimeKind.Utc).AddTicks(6667) },
                new WomanCategory { Id = 7, Name = "Footwear",      CreatedAt = new DateTime(2026, 6, 5, 18, 40, 53, 636, DateTimeKind.Utc).AddTicks(6667) },
                new WomanCategory { Id = 8, Name = "Accessories",   CreatedAt = new DateTime(2026, 6, 5, 18, 40, 53, 636, DateTimeKind.Utc).AddTicks(6667) },
                new WomanCategory { Id = 9, Name = "Sale",          CreatedAt = new DateTime(2026, 6, 6, 14, 43, 54, 356, DateTimeKind.Utc).AddTicks(6667) }
            );

        }
    }
}
