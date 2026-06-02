// Data/FileUploadDbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class FileUploadDbContextFactory : IDesignTimeDbContextFactory<FileUploadDbContext>
{
    public FileUploadDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FileUploadDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\Maya;Database=FileUploadDb;Trusted_Connection=True;TrustServerCertificate=True;");

        return new FileUploadDbContext(optionsBuilder.Options);
    }
}