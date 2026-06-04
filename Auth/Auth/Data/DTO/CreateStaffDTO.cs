namespace Auth.Data.DTO
{
    public class CreateStaffDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string RoleType { get; set; }
    }
}