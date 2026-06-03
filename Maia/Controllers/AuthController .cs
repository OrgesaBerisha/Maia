using Maia.Data.DTO;
using Maia.Data.Interface;
using Maia.Models.NoSQL;
using Microsoft.AspNetCore.Mvc;

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
    }
}
