namespace Auth.Data.DTO
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? RoleType { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DisabledAt { get; set; }
    }
}
