using Auth.Data;
using Auth.Data.DTO;
using Auth.Data.Interface;
using Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _dbContext;

        public UserService(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserDTO?> GetUser(int id)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.UserID == id);

            if (user == null) return null;

            return MapToDTO(user);
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            return await _dbContext.Users
                .Include(u => u.Role)
                .IgnoreQueryFilters()
                .Select(u => new UserDTO
                {
                    UserID = u.UserID,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    CreatedAt = u.CreatedAt,
                    RoleType = u.Role != null ? u.Role.RoleType : null,
                    IsActive = u.IsActive,
                    DisabledAt = u.DisabledAt
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<UserDTO>> GetAllCustomers()
        {
            return await _dbContext.Users
                .Include(u => u.Role)
                .IgnoreQueryFilters()
                .Where(u => u.Role != null && u.Role.RoleType == Roles.Customer)
                .Select(u => new UserDTO
                {
                    UserID = u.UserID,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    CreatedAt = u.CreatedAt,
                    RoleType = u.Role.RoleType,
                    IsActive = u.IsActive,
                    DisabledAt = u.DisabledAt
                })
                .ToListAsync();
        }

        public async Task DeleteUser(int id)
        {
            var user = await _dbContext.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.UserID == id);

            if (user == null)
                throw new Exception("User not found.");

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserDTO?> UpdateUser(int id, UserRegisterDTO request)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.UserID == id);

            if (user == null) return null;

            // FIXED: check email uniqueness before allowing update
            var emailTaken = await _dbContext.Users
                .AnyAsync(u => u.Email == request.Email && u.UserID != id);

            if (emailTaken)
                throw new Exception("Email is already in use by another account.");

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;

            await _dbContext.SaveChangesAsync();

            return MapToDTO(user);
        }

        public async Task<UserDTO?> UpdateUserRole(int userId, int newRoleId)
        {
            var user = await _dbContext.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.UserID == userId);

            if (user == null) return null;

            var role = await _dbContext.Roles.FindAsync(newRoleId)
                ?? throw new Exception("Role not found.");

            user.RoleID = newRoleId;
            await _dbContext.SaveChangesAsync();

            return new UserDTO
            {
                UserID = user.UserID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                RoleType = role.RoleType,
                IsActive = user.IsActive,
                DisabledAt = user.DisabledAt
            };
        }

        public async Task<UserDTO?> SetUserActiveStatus(int userId, bool isActive)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.UserID == userId);

            if (user == null) return null;

            user.IsActive = isActive;
            user.DisabledAt = isActive ? null : DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return MapToDTO(user);
        }

        // Shared mapping helper — avoids repeating the same 8-field block everywhere
        private static UserDTO MapToDTO(User user) => new UserDTO
        {
            UserID = user.UserID,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            RoleType = user.Role?.RoleType,
            IsActive = user.IsActive,
            DisabledAt = user.DisabledAt
        };
    }
}