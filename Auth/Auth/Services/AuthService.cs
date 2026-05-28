using Auth.Data;
using Auth.Data.DTO;
using Auth.Data.Interface;
using Auth.Models;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Mapster;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<UserDTO> Register(UserRegisterDTO request)
        {
            var existing = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existing != null)
                throw new ArgumentException("User with this email already exists.");

            var defaultRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleType == Roles.Customer);

            if (defaultRole == null)
                throw new Exception("Default role 'Customer' not found.");

            CreatePasswordHash(request.Password, out byte[] hash, out byte[] salt);

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                RoleID = defaultRole.RoleID,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user.Adapt<UserDTO>();
        }

        public async Task<string> Login(UserLoginDTO request)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                throw new ArgumentException("User not found.");

            if (!user.IsActive)
                throw new ArgumentException("User account is disabled.");

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                throw new ArgumentException("Incorrect password.");

            string refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();

            var accessToken = await CreateToken(user);

            return $"{accessToken}|||{refreshToken}";
        }

        public async Task Logout(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return;

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null)
                return;

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;

            await _context.SaveChangesAsync();
        }

        public async Task<string> CreateToken(User user)
        {
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleID == user.RoleID);

            if (role == null)
                throw new Exception("Role not found for user.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Role, role.RoleType)
            };

            var secret = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(secret))
                throw new Exception("JWT secret key missing.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserDTO> GetUserFromJwt(string jwt)
        {
            if (string.IsNullOrWhiteSpace(jwt))
                return null;

            var handler = new JwtSecurityTokenHandler();

            JwtSecurityToken token;
            try
            {
                token = handler.ReadJwtToken(jwt);
            }
            catch
            {
                return null;
            }

            var userIdClaim = token.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return null;

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserID == userId);

            if (user == null || !user.IsActive)
                return null;

            return user.Adapt<UserDTO>();
        }

        public async Task<(string accessToken, string refreshToken)> RotateRefreshToken(string oldRefreshToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == oldRefreshToken);

            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                return (null, null);

            if (!user.IsActive)
                return (null, null);

            string newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            var newAccessToken = await CreateToken(user);

            await _context.SaveChangesAsync();

            return (newAccessToken, newRefreshToken);
        }

        private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] hash, byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computed.SequenceEqual(hash);
        }

        private string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes);
        }
        //private readonly DataContext _context;
        //private readonly IJwtService _jwt;
        //public AuthService(DataContext context, IJwtService jwt)
        //{
        //    _context = context;
        //    _jwt = jwt;
        //}

        //public async Task<string> Register(RegisterDto dto)
        //{
        //    var user = new User
        //    {
        //        FirstName = dto.FirstName,
        //        LastName = dto.LastName,
        //        Email = dto.Email,
        //        PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        //    };

        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return _jwt.GenerateToken(user);
        //}

        //public async Task<string> Login(LoginDto dto)
        //{
        //    var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

        //    if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        //        throw new Exception("Invalid credentials");

        //    return _jwt.GenerateToken(user);
        //}

    }
}
