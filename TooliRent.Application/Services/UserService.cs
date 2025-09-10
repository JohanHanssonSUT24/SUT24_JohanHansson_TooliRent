using System.Threading.Tasks;
using TooliRent.Application.DTOs;
using TooliRent.Application.Interfaces.Services;
using TooliRent.Domain.Entities;
using TooliRent.Domain.Interfaces.Repositories;


namespace TooliRent.Application.Services
{
    public class UserService : IUserService
    {
        private readonly TooliRentDbContext _context;
        public UserService(TooliRentDbContext context)
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
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}