namespace Auth.Data.DTO
{
    public class UpdateUserDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? Password { get; set; }
    }
}
