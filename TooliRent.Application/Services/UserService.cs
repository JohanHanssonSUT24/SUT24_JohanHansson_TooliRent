using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooliRent.Application.Interfaces.Services;
using TooliRent.Application.DTOs;
using TooliRent.Domain.Entities;


namespace TooliRent.Application.Services
{
    public class UserService : IUserService
    {

        private readonly TooliRentDbContext _context;
        public UserService(TooliRentDbContext context)
        {
            _context = context;
        }
        public async Task<bool> RegisterUserAsync(RegisterUserDto registerDto)
        {
            var existingUser = await _context.Users.FirstorDefaultAsync(u => u.Email == registerDto.Email);
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
