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

        public async Task<UserDTO> GetUser(int id)
        {
            try
            {
                var user = await _dbContext.Users
                    .Include(u => u.Role)
                    .IgnoreQueryFilters() // Include disabled users
                    .FirstOrDefaultAsync(u => u.UserID == id);

                if (user == null) return null;

                return new UserDTO
                {
                    UserID = user.UserID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                    RoleType = user.Role != null ? user.Role.RoleType : null,
                    IsActive = user.IsActive,
                    DisabledAt = user.DisabledAt
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("An error occurred while fetching the user.");
            }
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            try
            {
                return await _dbContext.Users
                    .Include(u => u.Role)
                    .IgnoreQueryFilters() // Include disabled users
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("An error occurred while fetching users.");
            }
        }

        public async Task<IEnumerable<UserDTO>> GetAllCustomers()
        {
            try
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
                        RoleType = u.Role != null ? u.Role.RoleType : null,
                        IsActive = u.IsActive,
                        DisabledAt = u.DisabledAt
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("An error occurred while fetching customer users.");
            }
        }

        public async Task DeleteUser(int id)
        {
            try
            {
                var user = await _dbContext.Users
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(u => u.UserID == id);

                if (user != null)
                {
                    _dbContext.Users.Remove(user);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("An error occurred while deleting the user.");
            }
        }

        public async Task<UserDTO> UpdateUser(int id, UserRegisterDTO request)
        {
            try
            {
                var user = await _dbContext.Users
                    .Include(u => u.Role)
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(u => u.UserID == id);

                if (user == null) return null;

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Email = request.Email;

                await _dbContext.SaveChangesAsync();

                return new UserDTO
                {
                    UserID = user.UserID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                    RoleType = user.Role != null ? user.Role.RoleType : null,
                    IsActive = user.IsActive,
                    DisabledAt = user.DisabledAt
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("An error occurred while updating the user.");
            }
        }

        public async Task<UserDTO> UpdateUserRole(int userId, int newRoleId)
        {
            try
            {
                var user = await _dbContext.Users
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(u => u.UserID == userId);
                if (user == null) return null;

                var role = await _dbContext.Roles.FindAsync(newRoleId);
                if (role == null) throw new Exception("Role not found.");

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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("An error occurred while updating the user role.");
            }
        }

        public async Task<UserDTO> SetUserActiveStatus(int userId, bool isActive)
        {
            var user = await _dbContext.Users
                .IgnoreQueryFilters()
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserID == userId);

            if (user == null) return null;

            user.IsActive = isActive;
            user.DisabledAt = isActive ? null : DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return new UserDTO
            {
                UserID = user.UserID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                RoleType = user.Role != null ? user.Role.RoleType : null,
                IsActive = user.IsActive,
                DisabledAt = user.DisabledAt
            };
        }
    }
}
