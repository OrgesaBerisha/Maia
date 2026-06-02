using Auth.Data.DTO;
using Auth.Models;

namespace Auth.Data.Interface
{
    public interface IAuthService
    {
        Task<UserDTO> Register(UserRegisterDTO request);
        Task<AuthResponseDTO> Login(UserLoginDTO request);
        Task<AuthResponseDTO?> RotateRefreshToken(string oldRefreshToken);
        Task Logout(string refreshToken);
        Task<UserDTO?> GetUserFromJwt(string jwt);
        // CreateToken removed — it's an internal implementation detail
    }
}
