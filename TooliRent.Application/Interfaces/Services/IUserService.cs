using TooliRent.Application.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TooliRent.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(RegisterUserDto registerDto);
        Task<string?> LoginUserAsync(LoginUserDto loginDto);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<bool> UpdateUserAsync(UpdateUserDto updateDto);
        Task<bool> DeleteUserAsync(int id);
    }
}