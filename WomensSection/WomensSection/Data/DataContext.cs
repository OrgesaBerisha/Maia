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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CardsWomen>()
                .ToTable("CardsWomen");

            modelBuilder.Entity<WomanCategory>()
                .ToTable("WomanCategories");

            modelBuilder.Entity<CardsWomen>()
                .HasOne(c => c.WomanCategory)
                .WithMany()
                .HasForeignKey(c => c.WomanCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CardsWomen>()
                .Property(x => x.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CardsWomen>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // ⚠️ FIX KRYESOR: mos përdor objekte “të pa stabilizuara”
            var seedDate = new DateTime(2026, 1, 1);

            modelBuilder.Entity<WomanCategory>().HasData(
                new WomanCategory { Id = 1, Name = "Dresses" },
                new WomanCategory { Id = 2, Name = "Shoes" },
                new WomanCategory { Id = 3, Name = "Jackets" },
                new WomanCategory { Id = 4, Name = "Bags" }
            );

            modelBuilder.Entity<CardsWomen>().HasData(
                new CardsWomen { Id = 1, Title = "Summer Dress", ImageUrl = "...", Price = 25, Description = "Light summer dress", WomanCategoryId = 1, CreatedAt = seedDate },
                new CardsWomen { Id = 2, Title = "Nike Shoes", ImageUrl = "...", Price = 80, Description = "Sport shoes", WomanCategoryId = 2, CreatedAt = seedDate },
                new CardsWomen { Id = 3, Title = "Winter Jacket", ImageUrl = "...", Price = 120, Description = "Warm jacket", WomanCategoryId = 3, CreatedAt = seedDate },
                new CardsWomen { Id = 4, Title = "Luxury Bag", ImageUrl = "...", Price = 200, Description = "Fashion bag", WomanCategoryId = 4, CreatedAt = seedDate }
            );
        }
    }
}