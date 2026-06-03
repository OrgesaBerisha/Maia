using System.ComponentModel.DataAnnotations;

namespace Auth.Models
{
    public class PasswordResetToken
    {
        [Key]
        public int Id { get; set; }
        public int UserID { get; set; }
        public User User { get; set; } = null!;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
