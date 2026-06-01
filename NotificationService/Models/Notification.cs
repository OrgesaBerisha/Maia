// Models/Notification.cs
public class Notification
{
    public int Id { get; set; }
    public string UserId { get; set; }        // who receives it
    public string Title { get; set; }
    public string Message { get; set; }
    public string Type { get; set; }          // "email" | "inapp" | "push"
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}