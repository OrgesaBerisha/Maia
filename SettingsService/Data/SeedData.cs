using Microsoft.EntityFrameworkCore;

public static class SeedData
{
    public static async Task SeedAsync(SettingsDbContext db)
    {
        try
        {
            var hasSettings = await db.Settings.AnyAsync();
            if (hasSettings) return;

            db.Settings.AddRange(
                new Setting { Key = "site_name",          Value = "Maia Shop", Scope = "global" },
                new Setting { Key = "currency",           Value = "USD",       Scope = "global" },
                new Setting { Key = "max_upload_size_mb", Value = "5",         Scope = "global" },
                new Setting { Key = "maintenance_mode",   Value = "false",     Scope = "global" }
            );

            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Seed failed: {ex.Message}");
        }
    }
}
