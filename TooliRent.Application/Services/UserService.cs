using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TooliRent.Application.DTOs;
using TooliRent.Application.Interfaces.Services;
using TooliRent.Domain.Entities;
using TooliRent.Domain.Interfaces.Repositories;

namespace TooliRent.Application.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;


        public UserService(IUserRepository userRepository, IConfiguration configuration, IPasswordHasher<User> passwordHasher, IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<bool> RegisterUserAsync(RegisterUserDto registerDto)//Registers a new user after checking for existing email and hashing the password.
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
                Role = "User"
            };
            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, registerDto.Password);
            await _userRepository.AddAsync(newUser);

            return true;
        }
        public async Task<string?> LoginUserAsync(LoginUserDto loginDto)//Logs in a user by verifying credentials and generating a JWT token upon successful authentication.
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return null;
            }
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return null;
            }
            var token = GenerateJwtToken(user);
            return token;
        }
        private string GenerateJwtToken(User user)//Generates a JWT token containing user claims for authentication.
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var claims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            });
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()//Retrieves all users from the repository and maps them to UserDto objects.
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
        public async Task<UserDto?> GetUserByIdAsync(int id)//Retrieves a specific user by their ID and maps them to a UserDto object.
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;
            return _mapper.Map<UserDto>(user);
        }
        public async Task<bool> UpdateUserAsync(UpdateUserDto updateDto)//Updates an existing user's information by mapping the updated data from an UpdateUserDto object and saving the changes to the repository.
        {
            var userToUpdate = await _userRepository.GetByIdAsync(updateDto.Id);
            if (userToUpdate == null)
            {
                return false;

            }
            _mapper.Map(updateDto, userToUpdate);
            await _userRepository.UpdateAsync(userToUpdate);
            return true;
        }
        public async Task<bool> DeleteUserAsync(int id)//Deletes a user by their ID from the repository.
        {
            return await _userRepository.DeleteAsync(id);
        }
        public async Task<bool> ToggleUserStatusAsync(int userId, bool isActive)//Toggles a user's active status by their ID.
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            user.IsActive = isActive;
            await _userRepository.UpdateAsync(user);
            return true;
        }
    }
}