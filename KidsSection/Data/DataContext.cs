
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KidsCards>()
                .Property(x => x.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<KidsCategory>().HasData(
                new KidsCategory { Id = 1, Name = "T-Shirts" },
                new KidsCategory { Id = 2, Name = "Dresses" },
                new KidsCategory { Id = 3, Name = "Jeans" },
                new KidsCategory { Id = 4, Name = "Shorts" }
            );

            modelBuilder.Entity<KidsProductType>().HasData(
                new KidsSection.Models.KidsProductType { Id = 1, Name = "Boys" },
                new KidsSection.Models.KidsProductType { Id = 2, Name = "Girls" }
            );

            modelBuilder.Entity<KidsCards>().HasData(
                new KidsCards { Id = 1, Title = "Striped T-Shirt",  ImageUrl = "https://images.unsplash.com/photo-1519689373923-95a9fa831f2e?w=400&q=80", Price = 12, Description = "Colorful striped t-shirt",  KidsCategoryId = 1, KidsProductTypeId = 1 },
                new KidsCards { Id = 2, Title = "Floral Dress",     ImageUrl = "https://images.unsplash.com/photo-1508766917616-d22f3f1eea14?w=400&q=80", Price = 18, Description = "Pretty floral dress",        KidsCategoryId = 2, KidsProductTypeId = 2 },
                new KidsCards { Id = 3, Title = "Denim Jeans",      ImageUrl = "https://images.unsplash.com/photo-1503454537195-1dcabb73ffb9?w=400&q=80", Price = 22, Description = "Classic denim jeans",        KidsCategoryId = 3, KidsProductTypeId = 1 },
                new KidsCards { Id = 4, Title = "Summer Shorts",    ImageUrl = "https://images.unsplash.com/photo-1471286174890-9c112ac6f609?w=400&q=80", Price = 14, Description = "Light summer shorts",        KidsCategoryId = 4, KidsProductTypeId = 2 },
                new KidsCards { Id = 5, Title = "Rainbow Hoodie",   ImageUrl = "https://images.unsplash.com/photo-1556821840-3a63f15732ce?w=400&q=80", Price = 20, Description = "Fun rainbow hoodie",          KidsCategoryId = 1, KidsProductTypeId = 1 },
                new KidsCards { Id = 6, Title = "Princess Dress",   ImageUrl = "https://images.unsplash.com/photo-1518831959646-742c3a14ebf7?w=400&q=80", Price = 25, Description = "Lovely princess dress",      KidsCategoryId = 2, KidsProductTypeId = 2 },
                new KidsCards { Id = 7, Title = "Cargo Shorts",     ImageUrl = "https://images.unsplash.com/photo-1591195853828-11db59a44f43?w=400&q=80", Price = 16, Description = "Practical cargo shorts",     KidsCategoryId = 4, KidsProductTypeId = 1 },
                new KidsCards { Id = 8, Title = "Leggings",         ImageUrl = "https://images.unsplash.com/photo-1543087903-1ac2ec7aa8c5?w=400&q=80", Price = 15, Description = "Comfortable leggings",        KidsCategoryId = 3, KidsProductTypeId = 2 }
            );
        }
    }
}
