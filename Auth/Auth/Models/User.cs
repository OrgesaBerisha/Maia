using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int RoleID { get; set; }

        public Role Role { get; set; }

        // Stored as a hash, not plain text
        public string? RefreshTokenHash { get; set; }

        public DateTime? RefreshTokenExpiry { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? DisabledAt { get; set; }
    }
}
