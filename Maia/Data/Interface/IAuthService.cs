using Maia.Data.DTO;

namespace Maia.Data.Interface
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDto dto);
        Task<string> Login(LoginDto dto);
    }
}
