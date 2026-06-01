public class Setting
{
    public int Id { get; set; }
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
    public string Scope { get; set; } = "global";
    public string? OwnerId { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
