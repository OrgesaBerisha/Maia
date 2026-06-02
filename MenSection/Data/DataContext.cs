
using MenSection.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MenSection.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<MenCards> MenCards { get; set; }
        public DbSet<MenCategory> MenCategories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenCards>()
                .Property(x => x.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MenCategory>().HasData(
                new MenCategory { Id = 1, Name = "T-Shirts" },
                new MenCategory { Id = 2, Name = "Jackets" },
                new MenCategory { Id = 3, Name = "Jeans" },
                new MenCategory { Id = 4, Name = "Shirts" }
            );

            modelBuilder.Entity<MenCards>().HasData(
                new MenCards { Id = 1,  Title = "Classic White Tee",   ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=400&q=80",  Price = 20,  Description = "Essential white t-shirt",      MenCategoryId = 1 },
                new MenCards { Id = 2,  Title = "Black Graphic Tee",   ImageUrl = "https://images.unsplash.com/photo-1503341504253-dff4815485f1?w=400&q=80",  Price = 25,  Description = "Streetwear graphic t-shirt",    MenCategoryId = 1 },
                new MenCards { Id = 3,  Title = "Leather Jacket",      ImageUrl = "https://images.unsplash.com/photo-1551028719-00167b16eac5?w=400&q=80",     Price = 150, Description = "Classic leather jacket",        MenCategoryId = 2 },
                new MenCards { Id = 4,  Title = "Denim Jacket",        ImageUrl = "https://images.unsplash.com/photo-1523205771623-e0faa4d2813d?w=400&q=80",  Price = 89,  Description = "Casual denim jacket",           MenCategoryId = 2 },
                new MenCards { Id = 5,  Title = "Slim Fit Jeans",      ImageUrl = "https://images.unsplash.com/photo-1542272454315-4c01d7abdf4a?w=400&q=80",  Price = 55,  Description = "Modern slim fit jeans",         MenCategoryId = 3 },
                new MenCards { Id = 6,  Title = "Black Skinny Jeans",  ImageUrl = "https://images.unsplash.com/photo-1604176354204-9268737828e4?w=400&q=80",  Price = 60,  Description = "Sleek black skinny jeans",      MenCategoryId = 3 },
                new MenCards { Id = 7,  Title = "Oxford Shirt",        ImageUrl = "https://images.unsplash.com/photo-1596755094514-f87e34085b2c?w=400&q=80",  Price = 45,  Description = "Smart Oxford shirt",            MenCategoryId = 4 },
                new MenCards { Id = 8,  Title = "Flannel Shirt",       ImageUrl = "https://images.unsplash.com/photo-1589310243389-96a5483213a8?w=400&q=80",  Price = 38,  Description = "Casual flannel shirt",          MenCategoryId = 4 },
                new MenCards { Id = 9,  Title = "Bomber Jacket",       ImageUrl = "https://images.unsplash.com/photo-1520975916090-3105956dac38?w=400&q=80",  Price = 110, Description = "Stylish bomber jacket",         MenCategoryId = 2 },
                new MenCards { Id = 10, Title = "Linen Shirt",         ImageUrl = "https://images.unsplash.com/photo-1602810318383-e386cc2a3ccf?w=400&q=80",  Price = 42,  Description = "Light summer linen shirt",      MenCategoryId = 4 }
            );
        }
    }
}
