using Auth.Data.DTO;

namespace Auth.Data.Interface
{
    public interface IUserService
    {
        Task<UserDTO> GetUser(int id);
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task DeleteUser(int id);
        Task<UserDTO> UpdateUser(int id, UserRegisterDTO request);
        Task<IEnumerable<UserDTO>> GetAllCustomers();
        Task<UserDTO> UpdateUserRole(int userID, int newRoleID);
        Task<UserDTO> SetUserActiveStatus(int userID, bool isActive);
    }
}
