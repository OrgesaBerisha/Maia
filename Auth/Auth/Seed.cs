using Auth.Data;
using Auth.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Auth
{
    public class Seed
    {
        public static async Task SeedDataAsync(DataContext context)
        {
            // =========================
            // ROLES
            // =========================
            if (!await context.Roles.AnyAsync())
            {
                var roles = new List<Role>
                {
                    new Role { RoleType = Roles.Admin },
                    new Role { RoleType = Roles.SalesManager },
                    new Role { RoleType = Roles.WomenManager },
                    new Role { RoleType = Roles.MenManager },
                    new Role { RoleType = Roles.KidsManager },
                    new Role { RoleType = Roles.Customer }
                };

                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }
            else
            {
                // Ensure KidsManager role exists if DB was seeded before this role was added
                if (!await context.Roles.AnyAsync(r => r.RoleType == Roles.KidsManager))
                {
                    await context.Roles.AddAsync(new Role { RoleType = Roles.KidsManager });
                    await context.SaveChangesAsync();
                }
            }

            // =========================
            // SEED USERS
            // =========================
            await SeedUser(context, "admin@admin.com", "Super", "Admin", "Admin123!", Roles.Admin);
            await SeedUser(context, "sales@manager.com", "Sales", "Manager", "Sales123!", Roles.SalesManager);
            await SeedUser(context, "women@manager.com", "Women", "Manager", "Women123!", Roles.WomenManager);
            await SeedUser(context, "men@manager.com", "Men", "Manager", "Men123!", Roles.MenManager);
            await SeedUser(context, "kids@manager.com", "Kids", "Manager", "Kids123!", Roles.KidsManager);
        }

        // FIXED: null-guard on role lookup for all seeded users, not just admin
        private static async Task SeedUser(
            DataContext context,
            string email,
            string firstName,
            string lastName,
            string password,
            string roleType)
        {
            if (await context.Users.AnyAsync(u => u.Email == email))
                return;

            var role = await context.Roles.FirstOrDefaultAsync(r => r.RoleType == roleType)
                ?? throw new Exception($"Role '{roleType}' not found during seeding.");

            CreatePasswordHash(password, out byte[] hash, out byte[] salt);

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = hash,
                PasswordSalt = salt,
                RoleID = role.RoleID,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        private static void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}