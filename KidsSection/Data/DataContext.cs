
using KidsSection.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace KidsSection.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<KidsCards> KidsCards { get; set; }
        public DbSet<KidsCategory> KidsCategories { get; set; }
        public DbSet<KidsProductType> KidsProductTypes { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KidsCards>()
                .Property(x => x.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<KidsCategory>().HasData(
                new KidsCategory { Id = 1, Name = "Baby" },
                new KidsCategory { Id = 2, Name = "Girls" },
                new KidsCategory { Id = 3, Name = "Boys" },
                new KidsCategory { Id = 4, Name = "Sleepwear" },
                new KidsCategory { Id = 5, Name = "Swimwear" },
                new KidsCategory { Id = 6, Name = "Footwear" },
                new KidsCategory { Id = 7, Name = "Accessories" },
                new KidsCategory { Id = 8, Name = "Sale" }
            );

            modelBuilder.Entity<KidsProductType>().HasData(
                new KidsSection.Models.KidsProductType { Id = 1, Name = "Boys" },
                new KidsSection.Models.KidsProductType { Id = 2, Name = "Girls" }
            );

            modelBuilder.Entity<KidsCards>().HasData(
                // Baby
                new KidsCards { Id = 1,  Title = "Baby Striped Onesie",    ImageUrl = "https://images.unsplash.com/photo-1522771930-78848d9293e8?w=400&q=80", Price = 12, Description = "Soft cotton striped onesie",       KidsCategoryId = 1, KidsProductTypeId = 1 },
                new KidsCards { Id = 5,  Title = "Baby Rainbow Bodysuit",   ImageUrl = "https://images.unsplash.com/photo-1515488042361-ee00e0ddd4e4?w=400&q=80", Price = 14, Description = "Colourful rainbow bodysuit",     KidsCategoryId = 1, KidsProductTypeId = 2 },
                new KidsCards { Id = 9,  Title = "Baby Girl Romper",        ImageUrl = "https://images.unsplash.com/photo-1607227893600-9bc90f1ead82?w=400&q=80", Price = 18, Description = "Cute floral baby romper",        KidsCategoryId = 1, KidsProductTypeId = 2 },
                new KidsCards { Id = 10, Title = "Baby Boy Bodysuit",       ImageUrl = "https://images.unsplash.com/photo-1574179618836-b7b81b2e1a4d?w=400&q=80", Price = 16, Description = "Soft jersey baby bodysuit",      KidsCategoryId = 1, KidsProductTypeId = 1 },
                new KidsCards { Id = 11, Title = "Baby Knit Set",           ImageUrl = "https://images.unsplash.com/photo-1544367567-0f2fcb009e0b?w=400&q=80", Price = 22, Description = "Cosy knitted 2-piece set",        KidsCategoryId = 1, KidsProductTypeId = 2 },
                // Girls
                new KidsCards { Id = 2,  Title = "Girls Floral Dress",      ImageUrl = "https://images.unsplash.com/photo-1508766917616-d22f3f1eea14?w=400&q=80", Price = 28, Description = "Pretty summer floral dress",    KidsCategoryId = 2, KidsProductTypeId = 2 },
                new KidsCards { Id = 6,  Title = "Girls Princess Dress",    ImageUrl = "https://images.unsplash.com/photo-1518831959646-742c3a14ebf7?w=400&q=80", Price = 35, Description = "Elegant princess party dress",  KidsCategoryId = 2, KidsProductTypeId = 2 },
                new KidsCards { Id = 8,  Title = "Girls Leggings",          ImageUrl = "https://images.unsplash.com/photo-1543087903-1ac2ec7aa8c5?w=400&q=80", Price = 15, Description = "Comfortable stretch leggings",    KidsCategoryId = 2, KidsProductTypeId = 2 },
                new KidsCards { Id = 12, Title = "Girls Sundress",          ImageUrl = "https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?w=400&q=80", Price = 32, Description = "Light summer sundress",          KidsCategoryId = 2, KidsProductTypeId = 2 },
                new KidsCards { Id = 13, Title = "Girls Denim Skirt",       ImageUrl = "https://images.unsplash.com/photo-1496747611176-843222e1e57c?w=400&q=80", Price = 24, Description = "Classic denim mini skirt",       KidsCategoryId = 2, KidsProductTypeId = 2 },
                new KidsCards { Id = 14, Title = "Girls Puff Sleeve Top",   ImageUrl = "https://images.unsplash.com/photo-1566174053879-31528523f8ae?w=400&q=80", Price = 20, Description = "Trendy puff sleeve blouse",      KidsCategoryId = 2, KidsProductTypeId = 2 },
                // Boys
                new KidsCards { Id = 3,  Title = "Boys Denim Jeans",        ImageUrl = "https://images.unsplash.com/photo-1503454537195-1dcabb73ffb9?w=400&q=80", Price = 28, Description = "Classic straight-fit jeans",    KidsCategoryId = 3, KidsProductTypeId = 1 },
                new KidsCards { Id = 4,  Title = "Boys Summer Shorts",      ImageUrl = "https://images.unsplash.com/photo-1471286174890-9c112ac6f609?w=400&q=80", Price = 18, Description = "Light cotton summer shorts",     KidsCategoryId = 3, KidsProductTypeId = 1 },
                new KidsCards { Id = 7,  Title = "Boys Cargo Shorts",       ImageUrl = "https://images.unsplash.com/photo-1591195853828-11db59a44f43?w=400&q=80", Price = 20, Description = "Practical multi-pocket shorts",  KidsCategoryId = 3, KidsProductTypeId = 1 },
                new KidsCards { Id = 15, Title = "Boys Polo Shirt",         ImageUrl = "https://images.unsplash.com/photo-1519689373923-95a9fa831f2e?w=400&q=80", Price = 22, Description = "Classic cotton polo shirt",      KidsCategoryId = 3, KidsProductTypeId = 1 },
                new KidsCards { Id = 16, Title = "Boys Graphic Hoodie",     ImageUrl = "https://images.unsplash.com/photo-1556821840-3a63f15732ce?w=400&q=80", Price = 30, Description = "Cool graphic print hoodie",       KidsCategoryId = 3, KidsProductTypeId = 1 },
                new KidsCards { Id = 17, Title = "Boys Chino Pants",        ImageUrl = "https://images.unsplash.com/photo-1583744946564-b52ac1c389c8?w=400&q=80", Price = 26, Description = "Smart casual chino trousers",    KidsCategoryId = 3, KidsProductTypeId = 1 },
                new KidsCards { Id = 18, Title = "Boys Baseball Tee",       ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=400&q=80", Price = 16, Description = "Sporty baseball-style tee",       KidsCategoryId = 3, KidsProductTypeId = 1 },
                // Sleepwear
                new KidsCards { Id = 19, Title = "Girls Star Pyjamas",      ImageUrl = "https://images.unsplash.com/photo-1578662996442-48f60103fc96?w=400&q=80", Price = 22, Description = "Soft star-print pyjama set",     KidsCategoryId = 4, KidsProductTypeId = 2 },
                new KidsCards { Id = 20, Title = "Boys Dino Pyjamas",       ImageUrl = "https://images.unsplash.com/photo-1586105449897-20b5efeb3233?w=400&q=80", Price = 22, Description = "Fun dinosaur pyjama set",        KidsCategoryId = 4, KidsProductTypeId = 1 },
                new KidsCards { Id = 21, Title = "Baby Sleepsuit",          ImageUrl = "https://images.unsplash.com/photo-1567578301402-c43d6dbef7de?w=400&q=80", Price = 18, Description = "Warm fleece baby sleepsuit",      KidsCategoryId = 4, KidsProductTypeId = 1 },
                new KidsCards { Id = 22, Title = "Girls Nightgown",         ImageUrl = "https://images.unsplash.com/photo-1572804013309-59a88b7e92f1?w=400&q=80", Price = 20, Description = "Cosy cotton nightgown",          KidsCategoryId = 4, KidsProductTypeId = 2 },
                // Swimwear
                new KidsCards { Id = 23, Title = "Girls Swimsuit",          ImageUrl = "https://images.unsplash.com/photo-1541099649105-f69ad21f3246?w=400&q=80", Price = 28, Description = "Bright one-piece swimsuit",      KidsCategoryId = 5, KidsProductTypeId = 2 },
                new KidsCards { Id = 24, Title = "Boys Swim Shorts",        ImageUrl = "https://images.unsplash.com/photo-1507525428034-b723cf961d3e?w=400&q=80", Price = 20, Description = "Quick-dry swim shorts",          KidsCategoryId = 5, KidsProductTypeId = 1 },
                new KidsCards { Id = 25, Title = "Girls Bikini Set",        ImageUrl = "https://images.unsplash.com/photo-1550639525-c97d455acf70?w=400&q=80", Price = 30, Description = "Colourful two-piece bikini",       KidsCategoryId = 5, KidsProductTypeId = 2 },
                // Footwear
                new KidsCards { Id = 26, Title = "Kids Sneakers",           ImageUrl = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=400&q=80", Price = 38, Description = "Lightweight everyday sneakers",   KidsCategoryId = 6, KidsProductTypeId = 1 },
                new KidsCards { Id = 27, Title = "Girls Sandals",           ImageUrl = "https://images.unsplash.com/photo-1543163521-1bf539c55dd2?w=400&q=80", Price = 28, Description = "Strappy summer sandals",          KidsCategoryId = 6, KidsProductTypeId = 2 },
                new KidsCards { Id = 28, Title = "Boys Canvas Shoes",       ImageUrl = "https://images.unsplash.com/photo-1560769629-975ec94e6a86?w=400&q=80", Price = 32, Description = "Classic canvas lace-up shoes",    KidsCategoryId = 6, KidsProductTypeId = 1 },
                new KidsCards { Id = 29, Title = "Kids Rain Boots",         ImageUrl = "https://images.unsplash.com/photo-1597248881519-db089b3a0508?w=400&q=80", Price = 34, Description = "Waterproof rubber rain boots",   KidsCategoryId = 6, KidsProductTypeId = 2 },
                // Accessories
                new KidsCards { Id = 30, Title = "Kids School Backpack",    ImageUrl = "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=400&q=80", Price = 35, Description = "Spacious padded school bag",      KidsCategoryId = 7, KidsProductTypeId = 1 },
                new KidsCards { Id = 31, Title = "Girls Hair Bow Set",      ImageUrl = "https://images.unsplash.com/photo-1553361371-9b22f78e8b1d?w=400&q=80", Price = 12, Description = "Set of 5 colourful hair bows",    KidsCategoryId = 7, KidsProductTypeId = 2 },
                new KidsCards { Id = 32, Title = "Kids Sun Hat",            ImageUrl = "https://images.unsplash.com/photo-1485462537746-965f33f7f6a7?w=400&q=80", Price = 16, Description = "Wide-brim UV protection hat",    KidsCategoryId = 7, KidsProductTypeId = 1 },
                new KidsCards { Id = 33, Title = "Kids Sunglasses",         ImageUrl = "https://images.unsplash.com/photo-1499002238440-d264edd596ec?w=400&q=80", Price = 14, Description = "UV400 kids sunglasses",          KidsCategoryId = 7, KidsProductTypeId = 2 },
                // Sale
                new KidsCards { Id = 34, Title = "Baby Patterned Onesie",  ImageUrl = "https://images.unsplash.com/photo-1519689373923-95a9fa831f2e?w=400&q=80", Price = 20, Description = "Adorable patterned baby onesie", KidsCategoryId = 8, KidsProductTypeId = 1, DiscountPercent = 30 },
                new KidsCards { Id = 35, Title = "Girls Summer Dress",      ImageUrl = "https://images.unsplash.com/photo-1496747611176-843222e1e57c?w=400&q=80", Price = 35, Description = "Light floral girls dress",       KidsCategoryId = 8, KidsProductTypeId = 2, DiscountPercent = 25 },
                new KidsCards { Id = 36, Title = "Boys Zip Hoodie",         ImageUrl = "https://images.unsplash.com/photo-1471286174890-9c112ac6f609?w=400&q=80", Price = 32, Description = "Warm zip-up boys hoodie",        KidsCategoryId = 8, KidsProductTypeId = 1, DiscountPercent = 20 },
                new KidsCards { Id = 37, Title = "Kids Canvas Trainers",    ImageUrl = "https://images.unsplash.com/photo-1560769629-975ec94e6a86?w=400&q=80", Price = 40, Description = "Classic canvas trainers",         KidsCategoryId = 8, KidsProductTypeId = 2, DiscountPercent = 35 }
            );
        }
    }
}
