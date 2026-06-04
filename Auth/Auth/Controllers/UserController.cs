using Auth.Data.DTO;
using Auth.Data.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize] // All user management requires authentication
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            return Ok(new { Email = email, Role = role });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,SalesManager")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _service.GetUser(id);
            if (user == null)
                return NotFound(new { message = "User not found." });
            return Ok(user);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _service.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("customers")]
        [Authorize(Roles = "Admin,SalesManager,WomenManager,MenManager,KidsManager")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _service.GetAllCustomers();
            return Ok(customers);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _service.DeleteUser(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRegisterDTO request)
        {
            try
            {
                var updated = await _service.UpdateUser(id, request);
                if (updated == null)
                    return NotFound(new { message = "User not found." });
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleDTO request)
        {
            try
            {
                var updated = await _service.UpdateUserRole(request.UserID, request.NewRoleID);
                if (updated == null)
                    return NotFound(new { message = "User not found." });
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetUserStatus(int id, [FromBody] bool isActive)
        {
            var updated = await _service.SetUserActiveStatus(id, isActive);
            if (updated == null)
                return NotFound(new { message = "User not found." });
            return Ok(updated);
        }

        [HttpPost("staff")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateStaff([FromBody] CreateStaffDTO request)
        {
            try
            {
                var created = await _service.CreateStaffUser(request);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("roles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _service.GetAllRoles();
            return Ok(roles);
        }
    }
}