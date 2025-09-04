using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooliRent.Domain.Entities;

namespace TooliRent.Domain.Interfaces.Repositories
{
    public interface IToolRepository
    {
        Task<IEnumerable<Tool>> GetAllAsync();
        Task<Tool> GetByIdAsync(int id);
        Task AddAsync(Tool tool);
        Task UpdateAsync(Tool tool);
        Task<bool> DeleteAsync(int id);
    }
}
