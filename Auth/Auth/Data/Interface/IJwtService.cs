using Auth.Models;

namespace Auth.Data.Interface
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
