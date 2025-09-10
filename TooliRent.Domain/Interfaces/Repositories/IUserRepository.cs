using System.Threading.Tasks;
using TooliRent.Domain.Entities;

namespace TooliRent.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task AddAsync(User user);
    }
}