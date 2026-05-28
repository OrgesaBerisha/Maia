using Auth.Data.DTO;
using Auth.Data.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("UserMe")]
        //[Authorize]
        public IActionResult GetCurrentUser()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            return Ok(new { Email = email });
        }

        [HttpGet("GetUser")]
        //  [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _service.GetUser(id);

                if (user == null)
                    return NotFound("User not found.");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllUsers")]
        //  [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _service.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("deleteUser")]
        //   [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _service.DeleteUser(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("updateUser")]
        //  [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRegisterDTO request)
        {
            try
            {
                var updated = await _service.UpdateUser(id, request);

                if (updated == null)
                    return NotFound("User not found.");

                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllStudents")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                var customers = await _service.GetAllCustomers();

                if (customers == null || !customers.Any())
                    return NotFound("No customers found.");

                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving customers.");
            }
        }

        [HttpPut("updateUserRole")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleDTO request)
        {
            if (request == null)
                return BadRequest("Invalid request.");

            try
            {
                var updatedUser = await _service.UpdateUserRole(request.UserID, request.NewRoleID);

                if (updatedUser == null)
                    return NotFound("User or role not found.");

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





    }
}
