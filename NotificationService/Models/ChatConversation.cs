public class ChatConversation
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string Status { get; set; } = "Open";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastMessageAt { get; set; } = DateTime.UtcNow;
    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}
