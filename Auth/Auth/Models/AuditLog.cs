using System.ComponentModel.DataAnnotations;

namespace Auth.Models
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        public int? UserID { get; set; }
        public User? User { get; set; }

        [Required]
        public string Action { get; set; } = string.Empty;

        [Required]
        public string Entity { get; set; } = string.Empty;

        public string? EntityId { get; set; }

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }

        public string? IpAddress { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
