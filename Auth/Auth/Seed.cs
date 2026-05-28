using Auth.Data;
using Auth.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

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
                    new Role
                    {
                        RoleType = Roles.Admin
                    },

                    new Role
                    {
                        RoleType = Roles.SalesManager
                    },

                    new Role
                    {
                        RoleType = Roles.WomenManager
                    },

                    new Role
                    {
                        RoleType = Roles.MenManager
                    },

                    new Role
                    {
                        RoleType = Roles.Customer
                    }
                };

                await context.Roles.AddRangeAsync(roles);

                await context.SaveChangesAsync();
            }

            // =========================
            // ADMIN
            // =========================

            if (!await context.Users.AnyAsync(u => u.Email == "admin@admin.com"))
            {
                CreatePasswordHash(
                    "Admin123!",
                    out byte[] adminHash,
                    out byte[] adminSalt
                );

                var adminRole = await context.Roles
                    .FirstOrDefaultAsync(r => r.RoleType == Roles.Admin);

                var admin = new User
                {
                    FirstName = "Super",
                    LastName = "Admin",
                    Email = "admin@admin.com",

                    PasswordHash = adminHash,
                    PasswordSalt = adminSalt,

                    RoleID = adminRole.RoleID,

                    CreatedAt = DateTime.UtcNow,

                    IsActive = true
                };

                await context.Users.AddAsync(admin);

                await context.SaveChangesAsync();
            }

            // =========================
            // SALES MANAGER
            // =========================

            if (!await context.Users.AnyAsync(u => u.Email == "sales@manager.com"))
            {
                CreatePasswordHash(
                    "Sales123!",
                    out byte[] salesHash,
                    out byte[] salesSalt
                );

                var salesRole = await context.Roles
                    .FirstOrDefaultAsync(r => r.RoleType == Roles.SalesManager);

                var salesManager = new User
                {
                    FirstName = "Sales",
                    LastName = "Manager",
                    Email = "sales@manager.com",

                    PasswordHash = salesHash,
                    PasswordSalt = salesSalt,

                    RoleID = salesRole.RoleID,

                    CreatedAt = DateTime.UtcNow,

                    IsActive = true
                };

                await context.Users.AddAsync(salesManager);

                await context.SaveChangesAsync();
            }

            // =========================
            // WOMEN MANAGER
            // =========================

            if (!await context.Users.AnyAsync(u => u.Email == "women@manager.com"))
            {
                CreatePasswordHash(
                    "Women123!",
                    out byte[] womenHash,
                    out byte[] womenSalt
                );

                var womenRole = await context.Roles
                    .FirstOrDefaultAsync(r => r.RoleType == Roles.WomenManager);

                var womenManager = new User
                {
                    FirstName = "Women",
                    LastName = "Manager",
                    Email = "women@manager.com",

                    PasswordHash = womenHash,
                    PasswordSalt = womenSalt,

                    RoleID = womenRole.RoleID,

                    CreatedAt = DateTime.UtcNow,

                    IsActive = true
                };

                await context.Users.AddAsync(womenManager);

                await context.SaveChangesAsync();
            }

            // =========================
            // MEN MANAGER
            // =========================

            if (!await context.Users.AnyAsync(u => u.Email == "men@manager.com"))
            {
                CreatePasswordHash(
                    "Men123!",
                    out byte[] menHash,
                    out byte[] menSalt
                );

                var menRole = await context.Roles
                    .FirstOrDefaultAsync(r => r.RoleType == Roles.MenManager);

                var menManager = new User
                {
                    FirstName = "Men",
                    LastName = "Manager",
                    Email = "men@manager.com",

                    PasswordHash = menHash,
                    PasswordSalt = menSalt,

                    RoleID = menRole.RoleID,

                    CreatedAt = DateTime.UtcNow,

                    IsActive = true
                };

                await context.Users.AddAsync(menManager);

                await context.SaveChangesAsync();
            }
        }

        private static void CreatePasswordHash(
            string password,
            out byte[] hash,
            out byte[] salt
        )
        {
            using var hmac = new HMACSHA512();

            salt = hmac.Key;

            hash = hmac.ComputeHash(
                Encoding.UTF8.GetBytes(password)
            );
        }
    }
}
