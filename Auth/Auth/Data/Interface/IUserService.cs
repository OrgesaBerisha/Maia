using Auth.Data.DTO;

namespace Auth.Data.Interface
{
    public interface IUserService
    {
        Task<UserDTO?> GetUser(int id);
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task<IEnumerable<UserDTO>> GetAllCustomers();
        Task DeleteUser(int id);
        Task<UserDTO?> UpdateUser(int id, UpdateUserDTO request);
        Task<UserDTO?> UpdateUserRole(int userID, int newRoleID);
        Task<UserDTO?> SetUserActiveStatus(int userID, bool isActive);
        Task<UserDTO> CreateStaffUser(CreateStaffDTO request);
        Task<IEnumerable<RoleDTO>> GetAllRoles();
    }
}
