using Auth.Data.DTO;
using Auth.Data.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO request)
        {
            try
            {
                var userDto = await _service.Register(request);
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
            Response.Cookies.Append("jwt", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(30)
            });

            Response.Cookies.Append("refresh", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });
        }
    }
}