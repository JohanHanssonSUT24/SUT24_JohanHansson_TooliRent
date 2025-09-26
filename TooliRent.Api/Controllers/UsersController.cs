using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TooliRent.Application.DTOs;
using TooliRent.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace TooliRent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerDto)// Register new user
        {
            if (!ModelState.IsValid)// Check if model state is valid
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.RegisterUserAsync(registerDto);// Call service to register new user
            if (!result)
            {
                return BadRequest( new { message = "Registration failed. User may already exist."});
            }
            return Ok(new { message = "User registered successfully." });
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginDto)// User login
        {
            if (!ModelState.IsValid)// Check if model state is valid
            {
                return BadRequest(ModelState);
            }
            var token = await _userService.LoginUserAsync(loginDto);// Call service to login user and get JWT token
            if (token == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }
            return Ok(new { token });
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()// Get all users
        {
            var users = await _userService.GetAllUsersAsync();// Await the service to get all users
            return Ok(users);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)// Get user by id
        {
            var user = await _userService.GetUserByIdAsync(id);// Await the service to get user by id
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateDto)// Update existing user
        {
            if (id != updateDto.Id)// Check if id in URL matches id in body
            {
                return BadRequest(new { message = "User ID in URL and body do not match." });
            }
            var result = await _userService.UpdateUserAsync(updateDto);// Call service to update user
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)// Delete user by id
        {
            var result = await _userService.DeleteUserAsync(id);// Call service to delete user
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("{id}/activate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActivateUser(int id)// Activate user account
        {
            var success = await _userService.ToggleUserStatusAsync(id, true);// Call service to activate user
            if (success)
            {
                return Ok(new {message = "User activated successfully." });
            }
            return NotFound(new { message = "User not found." });
        }
        [HttpPost("{id}/deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeactivateUser(int id)// Deactivate user account
        {
            var success = await _userService.ToggleUserStatusAsync(id, false);// Call service to deactivate user
            if (success)
            {
                return Ok(new { message = "User deactivated successfully." });
            }
            return NotFound(new { message = "User not found." });
        }
    }
}
