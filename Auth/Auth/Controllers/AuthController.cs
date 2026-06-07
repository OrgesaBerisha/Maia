using Auth.Data.DTO;
using Auth.Data.Interface;
using Auth.Data;
using Auth.Models;
using Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace Auth.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly DataContext _context;
        private readonly IEmailService _email;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpClientFactory _http;
        private const string NotifUrl = "http://localhost:5151/api/notifications/send";

        public AuthController(IAuthService service, DataContext context, IEmailService email, IWebHostEnvironment env, IHttpClientFactory http)
        {
            _context = context;
            _email = email;
            _service = service;
            _env = env;
            _http = http;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO request)
        {
            try
            {
                var userDto = await _service.Register(request);

                _ = SendNotification(userDto.UserID.ToString(),
                    "Welcome to Maia!",
                    $"Hello {userDto.FirstName}, your account has been created. Start exploring our collections!");

                _ = _email.SendAsync(
                    userDto.Email,
                    "Welcome to MAIA — Verify Your Account",
                    $@"<div style='font-family:Georgia,serif;max-width:520px;margin:0 auto;background:#fbf8f5;padding:40px 32px;'>
                        <h1 style='font-size:36px;letter-spacing:6px;color:#1c0a06;margin:0 0 24px;'>MAIA</h1>
                        <p style='color:#7a5c52;font-size:14px;line-height:22px;margin:0 0 16px;'>
                            Hello {userDto.FirstName},
                        </p>
                        <p style='color:#7a5c52;font-size:14px;line-height:22px;margin:0 0 24px;'>
                            Thank you for creating your MAIA account. Your account is now active and ready to use.
                        </p>
                        <div style='background:#d4bab0;height:1px;margin:24px 0;'></div>
                        <p style='color:#aa9990;font-size:12px;margin:0;'>
                            If you did not create this account, please ignore this email.
                        </p>
                    </div>");

                return Ok(new { message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO request)
        {
            try
            {
                var tokens = await _service.Login(request);

                AppendAuthCookies(tokens.AccessToken, tokens.RefreshToken);

                return Ok(new
                {
                    isLoggedIn = true,
                    role = tokens.Role,
                    email = tokens.Email,
                    firstName = tokens.FirstName,
                    lastName = tokens.LastName
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refresh"];

            if (!string.IsNullOrEmpty(refreshToken))
                await _service.Logout(refreshToken);

            Response.Cookies.Delete("jwt");
            Response.Cookies.Delete("refresh");

            return Ok(new { message = "Logged out successfully." });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            // Try Authorization header first, then cookie
            string? jwt = null;

            if (Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var header = authHeader.ToString();
                if (header.StartsWith("Bearer "))
                    jwt = header.Substring("Bearer ".Length).Trim();
            }

            jwt ??= Request.Cookies["jwt"];

            if (string.IsNullOrEmpty(jwt))
                return Unauthorized(new { message = "JWT not found." });

            var user = await _service.GetUserFromJwt(jwt);
            if (user == null)
                return Unauthorized(new { message = "Invalid or expired token." });

            return Ok(user);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var oldRefreshToken = Request.Cookies["refresh"]?.Trim();

            if (string.IsNullOrEmpty(oldRefreshToken))
                return Unauthorized(new { message = "Refresh token not found." });

            // FIXED: RotateRefreshToken now returns AuthResponseDTO? instead of a tuple
            var tokens = await _service.RotateRefreshToken(oldRefreshToken);

            if (tokens == null)
                return Unauthorized(new { message = "Refresh token is invalid or expired." });

            AppendAuthCookies(tokens.AccessToken, tokens.RefreshToken);

            return Ok(new { message = "Token refreshed successfully." });
        }

        // ================= HELPER =================
        private void AppendAuthCookies(string accessToken, string refreshToken)
        {
            // Use Secure=false and SameSite=Lax so cookies work over HTTP in local dev.
            // The auth service runs without HTTPS in development, so Secure=true would
            // cause the browser to silently drop the cookie on every HTTP request.
            var shared = new CookieOptions
            {
                HttpOnly = true,
                Secure   = Request.IsHttps,
                SameSite = Request.IsHttps ? SameSiteMode.None : SameSiteMode.Lax,
            };

            Response.Cookies.Append("jwt", accessToken, new CookieOptions
            {
                HttpOnly = shared.HttpOnly,
                Secure   = shared.Secure,
                SameSite = shared.SameSite,
                Expires  = DateTime.UtcNow.AddMinutes(30)
            });

            Response.Cookies.Append("refresh", refreshToken, new CookieOptions
            {
                HttpOnly = shared.HttpOnly,
                Secure   = shared.Secure,
                SameSite = shared.SameSite,
                Expires  = DateTime.UtcNow.AddDays(7)
            });
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return Ok(new { message = "Nëse email ekziston, do të marrësh link resetimi." });

            var token = Guid.NewGuid().ToString("N");

            _context.PasswordResetTokens.Add(new PasswordResetToken
            {
                UserID = user.UserID,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            });
            await _context.SaveChangesAsync();

            var resetLink = $"http://localhost:5173/reset-password?token={token}";
            var body = $@"
                <h2>Resetimi i fjalëkalimit — MAIA</h2>
                <p>Kliko linkun më poshtë për të resetuar fjalëkalimin tënd:</p>
                <a href='{resetLink}' style='background:#1a1208;color:#fff;padding:12px 24px;text-decoration:none;'>
                    RESETO FJALËKALIMIN
                </a>
                <p>Linku skadon pas 1 ore.</p>
                <p>Nëse nuk e ke kërkuar ti këtë, inoro këtë email.</p>";

            try
            {
                await _email.SendAsync(user.Email, "Resetimi i fjalëkalimit — MAIA", body);
                return Ok(new { message = "Email u dërgua. Kontrollo kutinë tënde." });
            }
            catch
            {
                // Development fallback: return the link directly
                return Ok(new { message = $"Email config mungon. Përdor këtë link direkt: {resetLink}" });
            }
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var resetToken = await _context.PasswordResetTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Token == dto.Token && !t.IsUsed && t.ExpiresAt > DateTime.UtcNow);

            if (resetToken == null)
                return BadRequest(new { message = "Linku është i pavlefshëm ose ka skaduar." });

            using var hmac = new System.Security.Cryptography.HMACSHA512();
            resetToken.User.PasswordSalt = hmac.Key;
            resetToken.User.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(dto.NewPassword));
            resetToken.IsUsed = true;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Fjalëkalimi u ndryshua me sukses." });
        }

        private async Task SendNotification(string userId, string title, string message)
        {
            try
            {
                var client = _http.CreateClient();
                var payload = JsonSerializer.Serialize(new { userId, title, message, type = "inapp" });
                await client.PostAsync(NotifUrl,
                    new StringContent(payload, Encoding.UTF8, "application/json"));
            }
            catch { /* notification failure should not break auth flow */ }
        }
    }
}