using Microsoft.EntityFrameworkCore;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
        : base(options) { }

    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<ChatConversation> ChatConversations => Set<ChatConversation>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Notification>().HasIndex(n => n.UserId);
        builder.Entity<ChatConversation>().HasIndex(c => c.UserId);
        builder.Entity<ChatMessage>()
            .HasOne(m => m.Conversation)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ConversationId);
    }
}