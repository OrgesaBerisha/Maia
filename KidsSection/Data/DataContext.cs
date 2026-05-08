
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
