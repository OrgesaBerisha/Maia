// Contracts/NotificationEvent.cs
public class NotificationEvent
{
    public string UserId { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public string Type { get; set; }   // "email" | "inapp"
    public string? Email { get; set; } // required when Type = "email"
}