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
        public async Task<IActionResult> Register(UserRegisterDTO request)
        {
            try
            {
                var userDto = await _service.Register(request);
                if (userDto == null)
                    return BadRequest(new { message = "User registration failed" });

                return Ok(new { message = "User registered successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Registration failed: " + ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO request)
        {
            try
            {
                var tokens = await _service.Login(request);
                var split = tokens.Split("|||");

                if (split.Length != 2)
                    return StatusCode(500, new { message = "Token generation failed." });

                var accessToken = split[0];
                var refreshToken = split[1];

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

                return Ok(new
                {
                    isLoggedIn = true,
                    accessToken,
                    refreshToken
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "Internal server error." });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refresh"];
            await _service.Logout(refreshToken);

            Response.Cookies.Delete("jwt");
            Response.Cookies.Delete("refresh");

            return Ok(new { message = "Logged out successfully." });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            string jwt = null;

            if (Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                if (authHeader.ToString().StartsWith("Bearer "))
                    jwt = authHeader.ToString().Substring("Bearer ".Length);
            }

            jwt ??= Request.Cookies["jwt"];

            if (string.IsNullOrEmpty(jwt))
                return Unauthorized(new { message = "JWT not found" });

            var user = await _service.GetUserFromJwt(jwt);
            if (user == null)
                return Unauthorized(new { message = "Invalid token or user not found" });

            return Ok(user);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var oldRefreshToken = Request.Cookies["refresh"]?.Trim();

            if (string.IsNullOrEmpty(oldRefreshToken))
                return Unauthorized(new { message = "Refresh token not found" });

            var (newAccessToken, newRefreshToken) =
                await _service.RotateRefreshToken(oldRefreshToken);

            if (newAccessToken == null)
                return Unauthorized(new { message = "Refresh token invalid or expired" });

            Response.Cookies.Append("jwt", newAccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(30)
            });

            Response.Cookies.Append("refresh", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new
            {
                message = "Token refreshed",
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });


            //private readonly IAuthService _auth;

            //public AuthController(IAuthService auth)
            //{
            //    _auth = auth;
            //}

            //[HttpPost("register")]
            //public async Task<IActionResult> Register(RegisterDto dto)
            //{
            //    var token = await _auth.Register(dto);
            //    return Ok(token);
            //}

            //[HttpPost("login")]
            //public async Task<IActionResult> Login(LoginDto dto)
            //{
            //    var token = await _auth.Login(dto);
            //    return Ok(token);
            //}
        }
    }
}
