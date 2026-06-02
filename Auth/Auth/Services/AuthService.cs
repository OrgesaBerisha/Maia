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

        // ================= REGISTER =================
        public async Task<UserDTO> Register(UserRegisterDTO request)
        {
            var existing = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existing != null)
                throw new Exception("User already exists");

            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleType == Roles.Customer);

            if (role == null)
                throw new Exception("Default role missing");

            CreatePasswordHash(request.Password, out byte[] hash, out byte[] salt);

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                RoleID = role.RoleID,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user.Adapt<UserDTO>();
        }

        // ================= LOGIN (FIXED) =================
        public async Task<AuthResponseDTO> Login(UserLoginDTO request)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                throw new Exception("User not found");

            if (!user.IsActive)
                throw new Exception("User disabled");

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Wrong password");

            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();

            var accessToken = await CreateToken(user);

            return new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        // ================= CREATE TOKEN =================
        public async Task<string> CreateToken(User user)
        {
            var role = await _context.Roles.FindAsync(user.RoleID);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Role, role.RoleType)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // ================= REFRESH TOKEN =================
        public async Task<(string accessToken, string refreshToken)> RotateRefreshToken(string oldRefreshToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == oldRefreshToken);

            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                return (null, null);

            var newRefresh = GenerateRefreshToken();

            user.RefreshToken = newRefresh;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            var newAccess = await CreateToken(user);

            await _context.SaveChangesAsync();

            return (newAccess, newRefresh);
        }

        // ================= LOGOUT =================
        public async Task Logout(string refreshToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null) return;

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;

            await _context.SaveChangesAsync();
        }

        // ================= JWT PARSE =================
        public async Task<UserDTO> GetUserFromJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            var userId = token.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return null;

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserID == int.Parse(userId));

            return user?.Adapt<UserDTO>();
        }

        // ================= HELPERS =================
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
