using System.ComponentModel.DataAnnotations;

namespace Auth.Models
{
    public class RolePermission
    {
        [Key]
        public int Id { get; set; }

        public int RoleID { get; set; }
        public Role Role { get; set; } = null!;

        public int PermissionId { get; set; }
        public Permission Permission { get; set; } = null!;
    }
}
