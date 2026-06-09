using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

public class ChatHub : Hub
{
    private readonly NotificationDbContext _db;

    public ChatHub(NotificationDbContext db)
    {
        _db = db;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        var role = Context.User?.FindFirstValue(ClaimTypes.Role) ?? "";

        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");

            if (IsStaff(role))
                await Groups.AddToGroupAsync(Context.ConnectionId, "staff");
        }

        await base.OnConnectedAsync();
    }

    public async Task SendMessage(int conversationId, string content)
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        var userName = Context.User?.FindFirstValue(ClaimTypes.Name)
                       ?? Context.User?.FindFirstValue("name")
                       ?? "User";
        var role = Context.User?.FindFirstValue(ClaimTypes.Role) ?? "";
        var isStaff = IsStaff(role);

        var conv = await _db.ChatConversations.FindAsync(conversationId);
        if (conv == null) return;

        var msg = new ChatMessage
        {
            ConversationId = conversationId,
            SenderId       = userId,
            SenderName     = userName,
            IsStaff        = isStaff,
            Content        = content,
            SentAt         = DateTime.UtcNow,
        };

        _db.ChatMessages.Add(msg);
        conv.LastMessageAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        // send to customer
        await Clients.Group($"user-{conv.UserId}").SendAsync("ReceiveMessage", msg);
        // send to all staff
        await Clients.Group("staff").SendAsync("ReceiveMessage", msg);
    }

    private static bool IsStaff(string role) =>
        role is "Admin" or "SalesManager" or "WomenManager" or "MenManager" or "KidsManager";
}
