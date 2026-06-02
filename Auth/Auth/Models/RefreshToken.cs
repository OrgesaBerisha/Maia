using System.ComponentModel.DataAnnotations;

namespace Auth.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        public int UserID { get; set; }
        public User User { get; set; } = null!;

        [Required]
        public string TokenHash { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsRevoked => RevokedAt.HasValue;
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
