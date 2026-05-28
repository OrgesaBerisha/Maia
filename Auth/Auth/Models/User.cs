using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public byte[]? PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int RoleID { get; set; }
        public Role Role { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? DisabledAt { get; set; }
    }
}
