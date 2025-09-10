using System.Threading.Tasks;
using TooliRent.Application.DTOs;
using TooliRent.Application.Interfaces.Services;
using TooliRent.Domain.Entities;
using TooliRent.Domain.Interfaces.Repositories;

namespace TooliRent.Application.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;


        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterUserAsync(RegisterUserDto registerDto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return false;
            }

            var newUser = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                PasswordHash = registerDto.Password,
                Role = "User"
            };

            await _userRepository.AddAsync(newUser);

            return true;
        }
    }
}