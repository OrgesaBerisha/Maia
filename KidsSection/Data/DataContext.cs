
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
                new KidsCards { Id = 1, Title = "Striped T-Shirt", ImageUrl = "https://picsum.photos/seed/kidstshirt/400/500", Price = 12, Description = "Colorful striped t-shirt", KidsCategoryId = 1, KidsProductTypeId = 1 },
                new KidsCards { Id = 2, Title = "Floral Dress", ImageUrl = "https://picsum.photos/seed/kidsdress/400/500", Price = 18, Description = "Pretty floral dress", KidsCategoryId = 2, KidsProductTypeId = 2 },
                new KidsCards { Id = 3, Title = "Denim Jeans", ImageUrl = "https://picsum.photos/seed/kidsjeans/400/500", Price = 22, Description = "Classic denim jeans", KidsCategoryId = 3, KidsProductTypeId = 1 },
                new KidsCards { Id = 4, Title = "Summer Shorts", ImageUrl = "https://picsum.photos/seed/kidsshorts/400/500", Price = 14, Description = "Light summer shorts", KidsCategoryId = 4, KidsProductTypeId = 2 }
            );
        }
    }
}
