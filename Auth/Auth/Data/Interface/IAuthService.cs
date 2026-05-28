using Auth.Data.DTO;
using Auth.Models;

namespace Auth.Data.Interface
{
    public interface IAuthService
    {
        Task<UserDTO> Register(UserRegisterDTO request);
        Task<string> Login(UserLoginDTO request);
        public Task<string> CreateToken(User user);
        Task<(string accessToken, string refreshToken)> RotateRefreshToken(string oldRefreshToken);
        Task Logout(string refreshToken);
        Task<UserDTO> GetUserFromJwt(string jwt);
        //Task<string> Register(RegisterDto dto);
        //Task<string> Login(LoginDto dto);
    }
}
