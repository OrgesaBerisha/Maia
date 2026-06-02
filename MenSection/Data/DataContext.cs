
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
                // T-Shirts
                new MenCards { Id = 1,  Title = "Classic White Tee",     ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=400&q=80",  Price = 20,  Description = "Essential white t-shirt for everyday wear",           MenCategoryId = 1 },
                new MenCards { Id = 2,  Title = "Black Graphic Tee",     ImageUrl = "https://images.unsplash.com/photo-1503341504253-dff4815485f1?w=400&q=80",  Price = 25,  Description = "Bold streetwear graphic t-shirt",                     MenCategoryId = 1 },
                new MenCards { Id = 3,  Title = "Navy Blue Tee",         ImageUrl = "https://images.unsplash.com/photo-1583743814966-8936f5b7be1a?w=400&q=80",  Price = 22,  Description = "Classic navy blue crew neck t-shirt",                 MenCategoryId = 1 },
                new MenCards { Id = 4,  Title = "Striped Polo Shirt",    ImageUrl = "https://images.unsplash.com/photo-1581655353564-df123a1eb820?w=400&q=80",  Price = 35,  Description = "Smart striped polo for casual outings",               MenCategoryId = 1 },
                // Jackets
                new MenCards { Id = 5,  Title = "Leather Jacket",        ImageUrl = "https://images.unsplash.com/photo-1551028719-00167b16eac5?w=400&q=80",     Price = 150, Description = "Classic black leather jacket with zip closure",       MenCategoryId = 2 },
                new MenCards { Id = 6,  Title = "Denim Jacket",          ImageUrl = "https://images.unsplash.com/photo-1523205771623-e0faa4d2813d?w=400&q=80",  Price = 89,  Description = "Timeless casual denim jacket",                        MenCategoryId = 2 },
                new MenCards { Id = 7,  Title = "Bomber Jacket",         ImageUrl = "https://images.unsplash.com/photo-1520975916090-3105956dac38?w=400&q=80",  Price = 110, Description = "Stylish bomber jacket perfect for autumn",            MenCategoryId = 2 },
                new MenCards { Id = 8,  Title = "Puffer Jacket",         ImageUrl = "https://images.unsplash.com/photo-1547624643-3bf761b09502?w=400&q=80",     Price = 130, Description = "Warm puffer jacket for cold winter days",             MenCategoryId = 2 },
                // Jeans
                new MenCards { Id = 9,  Title = "Slim Fit Jeans",        ImageUrl = "https://images.unsplash.com/photo-1542272454315-4c01d7abdf4a?w=400&q=80",  Price = 55,  Description = "Modern slim fit blue jeans",                          MenCategoryId = 3 },
                new MenCards { Id = 10, Title = "Black Skinny Jeans",    ImageUrl = "https://images.unsplash.com/photo-1604176354204-9268737828e4?w=400&q=80",  Price = 60,  Description = "Sleek black skinny jeans for a sharp look",           MenCategoryId = 3 },
                new MenCards { Id = 11, Title = "Relaxed Fit Jeans",     ImageUrl = "https://images.unsplash.com/photo-1473966968600-fa801b869a1a?w=400&q=80",  Price = 50,  Description = "Comfortable relaxed fit jeans for casual days",       MenCategoryId = 3 },
                new MenCards { Id = 12, Title = "Ripped Jeans",          ImageUrl = "https://images.unsplash.com/photo-1555689502-c4b22d76c56f?w=400&q=80",     Price = 65,  Description = "Trendy ripped jeans with distressed details",         MenCategoryId = 3 },
                // Shirts
                new MenCards { Id = 13, Title = "Oxford Shirt",          ImageUrl = "https://images.unsplash.com/photo-1596755094514-f87e34085b2c?w=400&q=80",  Price = 45,  Description = "Smart Oxford shirt for office or casual wear",        MenCategoryId = 4 },
                new MenCards { Id = 14, Title = "Flannel Shirt",         ImageUrl = "https://images.unsplash.com/photo-1589310243389-96a5483213a8?w=400&q=80",  Price = 38,  Description = "Cozy flannel shirt perfect for weekends",             MenCategoryId = 4 },
                new MenCards { Id = 15, Title = "Linen Shirt",           ImageUrl = "https://images.unsplash.com/photo-1602810318383-e386cc2a3ccf?w=400&q=80",  Price = 42,  Description = "Light breathable linen shirt for summer",             MenCategoryId = 4 },
                new MenCards { Id = 16, Title = "Black Formal Shirt",    ImageUrl = "https://images.unsplash.com/photo-1598033129183-c4f50c736f10?w=400&q=80",  Price = 48,  Description = "Elegant black formal shirt for special occasions",    MenCategoryId = 4 }
            );
        }
    }
}
