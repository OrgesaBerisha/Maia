using Auth.Data;
using Auth.Data.DTO;
using Auth.Data.Interface;
using Auth.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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
                throw new Exception("User already exists.");

            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleType == Roles.Customer)
                ?? throw new Exception("Default customer role is missing. Please seed the database.");

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

            return new UserDTO
            {
                UserID = user.UserID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                RoleType = null,
                IsActive = user.IsActive
            };
        }

        // ================= LOGIN =================
        public async Task<AuthResponseDTO> Login(UserLoginDTO request)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            // Use a generic message — don't reveal whether email or password was wrong
            if (user == null || !VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Invalid email or password.");

            if (!user.IsActive)
                throw new Exception("This account has been disabled.");

            var refreshToken = GenerateRefreshToken();

            // FIXED: store the hash of the refresh token, not the raw value
            user.RefreshTokenHash = HashToken(refreshToken);
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();

            var accessToken = CreateToken(user);

            return new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Role = user.Role?.RoleType,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        // ================= CREATE TOKEN (private — not on interface) =================
        private string CreateToken(User user)
        {
            // user.Role is already loaded via .Include() in Login and RotateRefreshToken
            if (user.Role == null)
                throw new Exception("User role not loaded.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Role, user.Role.RoleType)
            };

            var keyString = _configuration["Jwt:Key"]
                ?? throw new Exception("JWT key is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
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
        public async Task<AuthResponseDTO?> RotateRefreshToken(string oldRefreshToken)
        {
            var hashedOld = HashToken(oldRefreshToken);

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.RefreshTokenHash == hashedOld);

            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow || !user.IsActive)
                return null;

            var newRefreshToken = GenerateRefreshToken();

            user.RefreshTokenHash = HashToken(newRefreshToken);
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            var newAccessToken = CreateToken(user);

            await _context.SaveChangesAsync();

            return new AuthResponseDTO
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        // ================= LOGOUT =================
        public async Task Logout(string refreshToken)
        {
            var hashed = HashToken(refreshToken);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshTokenHash == hashed);

            if (user == null) return;

            user.RefreshTokenHash = null;
            user.RefreshTokenExpiry = null;

            await _context.SaveChangesAsync();
        }

        // ================= GET USER FROM JWT =================
        public async Task<UserDTO?> GetUserFromJwt(string jwt)
        {
            // FIXED: properly validate the token instead of blindly reading it
            var keyString = _configuration["Jwt:Key"]
                ?? throw new Exception("JWT key is not configured.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

            try
            {
                tokenHandler.ValidateToken(jwt, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userIdClaim = jwtToken.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                    return null;

                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.UserID == userId);

                if (user == null) return null;

                return new UserDTO
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
            catch
            {
                return null; // Token is invalid or expired
            }
        }

        // ================= HELPERS =================
        private static void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] hash, byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computed.SequenceEqual(hash);
        }

        private static string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes);
        }

        private static string HashToken(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(bytes);
        }
    }
}