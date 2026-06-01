using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class NotificationDbContextFactory : IDesignTimeDbContextFactory<NotificationDbContext>
{
    public NotificationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<NotificationDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\Maya;Database=NotificationDb;Trusted_Connection=True;TrustServerCertificate=True;");

        return new NotificationDbContext(optionsBuilder.Options);
    }
}