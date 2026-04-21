using Maia.Models;

namespace Maia.Data.Interface
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
