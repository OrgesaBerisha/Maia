using System.ComponentModel.DataAnnotations;

namespace Auth.Models
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }

        public int UserID { get; set; }
        public User User { get; set; } = null!;

        public int RoleID { get; set; }
        public Role Role { get; set; } = null!;

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
