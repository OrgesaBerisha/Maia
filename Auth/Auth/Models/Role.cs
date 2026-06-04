using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Models
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleID { get; set; }

        [Required]
        public required string RoleType { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }

    public static class Roles
    {
        public const string Admin = "Admin";
        public const string SalesManager = "SalesManager";
        public const string WomenManager = "WomenManager";
        public const string MenManager = "MenManager";
        public const string KidsManager = "KidsManager";
        public const string Customer = "Customer";
    }
}
