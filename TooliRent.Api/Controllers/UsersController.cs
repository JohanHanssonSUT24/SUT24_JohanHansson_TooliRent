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
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.RegisterUserAsync(registerDto);
            if (!result)
            {
                return BadRequest( new { message = "Registration failed. User may already exist."});
            }
            return Ok(new { message = "User registered successfully." });
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = await _userService.LoginUserAsync(loginDto);
            if (token == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }
            return Ok(new { token });
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest(new { message = "User ID in URL and body do not match." });
            }
            var result = await _userService.UpdateUserAsync(updateDto);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
