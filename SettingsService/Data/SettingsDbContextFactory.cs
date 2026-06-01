using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class SettingsDbContextFactory : IDesignTimeDbContextFactory<SettingsDbContext>
{
    public SettingsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SettingsDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\Maya;Database=SettingsDb;Trusted_Connection=True;TrustServerCertificate=True;");

        return new SettingsDbContext(optionsBuilder.Options);
    }
}
