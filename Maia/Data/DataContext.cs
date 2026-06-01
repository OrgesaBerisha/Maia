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

        // 👇 USERS & ROLES
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        // 👇 WOMEN SHOP
        public DbSet<CardsWomen> CardsWoman { get; set; }
        public DbSet<WomanCategory> WomanCategories { get; set; }

        // 👇 KIDS
        public DbSet<KidsViewAllCards> KidsViewAllCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

          
            // 👇 WOMEN RELATION (ZARA STYLE)
      
            modelBuilder.Entity<CardsWomen>()
                .HasOne(c => c.WomanCategory)
                .WithMany()
                .HasForeignKey(c => c.WomanCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // 👇 OPTIONAL: CLEAN TABLE NAMES
           
            modelBuilder.Entity<CardsWomen>()
                .ToTable("CardsWomen");

            modelBuilder.Entity<WomanCategory>()
                .ToTable("WomanCategories");
        public DbSet<KidsCategory> KidsCategories { get; set; }


            modelBuilder.Entity<KidsViewAllCards>()
                .ToTable("KidsViewAllCards");
        }
    }
}