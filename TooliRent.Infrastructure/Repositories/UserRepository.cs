using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TooliRent.Domain.Entities;
using TooliRent.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using TooliRent.Infrastructure.Data;

namespace TooliRent.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TooliRentDbContext _context;

        public UserRepository(TooliRentDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailAsync(string email)//Retrieves a user by their email address from the database.
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)//Adds a new user to the database and saves changes.
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<User>> GetAllAsync()//Retrieves all users from the database.
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User?> GetByIdAsync(int id)//Retrieves a specific user by their ID from the database.
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task UpdateAsync(User user)//Updates an existing user in the database and saves changes.
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteAsync(int id)//Deletes a user by their ID from the database and saves changes.
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
