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

            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);

            modelBuilder.Entity<CardsWomen>().HasData(
                new CardsWomen { Id = 1,  Title = "Summer Dress",      ImageUrl = "https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?w=400&q=80", Price = 25m,  Description = "Light summer dress",          WomanCategoryId = 1, CreatedAt = seedDate },
                new CardsWomen { Id = 2,  Title = "White Sneakers",    ImageUrl = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=400&q=80", Price = 80m,  Description = "Classic white sneakers",      WomanCategoryId = 2, CreatedAt = seedDate },
                new CardsWomen { Id = 3,  Title = "Winter Jacket",     ImageUrl = "https://images.unsplash.com/photo-1591047139829-d91aecb6caea?w=400&q=80", Price = 120m, Description = "Warm winter jacket",          WomanCategoryId = 3, CreatedAt = seedDate },
                new CardsWomen { Id = 4,  Title = "Leather Bag",       ImageUrl = "https://images.unsplash.com/photo-1548036328-c9fa89d128fa?w=400&q=80", Price = 200m, Description = "Premium leather bag",         WomanCategoryId = 4, CreatedAt = seedDate },
                new CardsWomen { Id = 5,  Title = "Floral Midi Dress", ImageUrl = "https://images.unsplash.com/photo-1496747611176-843222e1e57c?w=400&q=80", Price = 35m,  Description = "Beautiful floral midi dress", WomanCategoryId = 1, CreatedAt = seedDate },
                new CardsWomen { Id = 6,  Title = "Blue Summer Dress", ImageUrl = "https://images.unsplash.com/photo-1572804013309-59a88b7e92f1?w=400&q=80", Price = 45m,  Description = "Elegant blue summer dress",   WomanCategoryId = 1, CreatedAt = seedDate },
                new CardsWomen { Id = 7,  Title = "Red Evening Dress", ImageUrl = "https://images.unsplash.com/photo-1550639525-c97d455acf70?w=400&q=80", Price = 89m,  Description = "Stunning red evening dress",  WomanCategoryId = 1, CreatedAt = seedDate },
                new CardsWomen { Id = 8,  Title = "White Mini Dress",  ImageUrl = "https://images.unsplash.com/photo-1566174053879-31528523f8ae?w=400&q=80", Price = 55m,  Description = "Chic white mini dress",       WomanCategoryId = 1, CreatedAt = seedDate },
                new CardsWomen { Id = 9,  Title = "Heel Sandals",      ImageUrl = "https://images.unsplash.com/photo-1543163521-1bf539c55dd2?w=400&q=80", Price = 95m,  Description = "Elegant heel sandals",        WomanCategoryId = 2, CreatedAt = seedDate },
                new CardsWomen { Id = 10, Title = "Black Ankle Boots", ImageUrl = "https://images.unsplash.com/photo-1560769629-975ec94e6a86?w=400&q=80", Price = 120m, Description = "Sleek black ankle boots",     WomanCategoryId = 2, CreatedAt = seedDate },
                new CardsWomen { Id = 11, Title = "Trench Coat",       ImageUrl = "https://images.unsplash.com/photo-1539109136-461e6d5a08f3?w=400&q=80", Price = 160m, Description = "Classic beige trench coat",   WomanCategoryId = 3, CreatedAt = seedDate },
                new CardsWomen { Id = 12, Title = "Crossbody Bag",     ImageUrl = "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=400&q=80", Price = 75m,  Description = "Stylish crossbody bag",       WomanCategoryId = 4, CreatedAt = seedDate }
            );
        }
    }
}
