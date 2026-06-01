// Data/FileUploadDbContext.cs
using Microsoft.EntityFrameworkCore;

public class FileUploadDbContext : DbContext
{
    public FileUploadDbContext(DbContextOptions<FileUploadDbContext> options)
        : base(options) { }

    public DbSet<FileRecord> FileRecords => Set<FileRecord>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<FileRecord>()
            .HasIndex(f => f.Category);
    }
}