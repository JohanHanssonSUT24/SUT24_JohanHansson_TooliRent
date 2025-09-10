using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TooliRent.Application.DTOs;
using TooliRent.Application.Interfaces.Services;

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
    }
}
