using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooliRent.Domain.Entities;

namespace TooliRent.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
    }
}
