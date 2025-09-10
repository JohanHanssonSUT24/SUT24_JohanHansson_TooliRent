using TooliRent.Application.DTOs;

namespace TooliRent.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(RegisterUserDto registerDto);
        Task<string?> LoginUserAsync(LoginUserDto loginDto);
    }
}