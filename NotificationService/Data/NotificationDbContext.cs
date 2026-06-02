using Microsoft.EntityFrameworkCore;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
        : base(options) { }

    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Notification>()
            .HasIndex(n => n.UserId);
    }
}