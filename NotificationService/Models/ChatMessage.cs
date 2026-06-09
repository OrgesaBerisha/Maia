public class ChatMessage
{
    public int Id { get; set; }
    public int ConversationId { get; set; }
    public ChatConversation Conversation { get; set; } = null!;
    public string SenderId { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public bool IsStaff { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }
}
