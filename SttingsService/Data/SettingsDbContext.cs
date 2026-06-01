// Data/SettingsDbContext.cs
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

public class SettingsDbContext : DbContext
{
    public SettingsDbContext(DbContextOptions<SettingsDbContext> options)
        : base(options) { }

    public DbSet<Setting> Settings => Set<Setting>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Setting>()
            .HasIndex(s => new { s.Key, s.OwnerId })
            .IsUnique();
    }
}