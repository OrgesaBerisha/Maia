// Models/FileRecord.cs
public class FileRecord
{
    public int Id { get; set; }
    public string FileName { get; set; } = "";
    public string ContentType { get; set; } = "";
    public string Url { get; set; } = "";
    public string UploadedBy { get; set; } = "";
    public string Category { get; set; } = "";  // "men" | "women" | "kids"
    public long SizeInBytes { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}