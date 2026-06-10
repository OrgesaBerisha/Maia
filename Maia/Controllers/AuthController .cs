using Maia.Data.DTO;
using Maia.Data.Interface;
using Maia.Models.NoSQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Maia.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly IAuditLogRepository _auditLog;

        public AuthController(IAuthService auth, IAuditLogRepository auditLog)
        {
            _auth = auth;
            _auditLog = auditLog;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var token = await _auth.Register(dto);

            await _auditLog.LogAsync(new AuditLogDocument
            {
                Action = "REGISTER",
                Entity = "User",
                CreatedBy = dto.Email,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
            });

            return Ok(token);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var token = await _auth.Login(dto);

                await _auditLog.LogAsync(new AuditLogDocument
                {
                    Action = "LOGIN",
                    Entity = "User",
                    CreatedBy = dto.Email,
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
                });

                return Ok(token);
            }
            catch
            {
                await _auditLog.LogAsync(new AuditLogDocument
                {
                    Action = "LOGIN_FAILED",
                    Entity = "User",
                    CreatedBy = dto.Email,
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
                });

                return Unauthorized("Invalid credentials");
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            await _auth.ForgotPassword(dto);
            return Ok(new { message = "If that email exists, a reset link has been sent." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            try
            {
                await _auth.ResetPassword(dto);
                return Ok(new { message = "Password reset successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
