using Maia.Data;
using Maia.Data.DTO;
using Maia.Data.Interface;
using Maia.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Maia.Services
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IJwtService _jwt;
        public AuthService(DataContext context, IJwtService jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        public async Task<string> Register(RegisterDto dto)
        {
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _jwt.GenerateToken(user);
        }

        public async Task<string> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            return _jwt.GenerateToken(user);
        }
    }
}
