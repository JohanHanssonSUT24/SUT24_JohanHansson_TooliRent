using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooliRent.Domain.Entities;

namespace TooliRent.Domain.Interfaces.Repositories
{
    public interface IToolCategoryRepository
    {
        Task<IEnumerable<ToolCategory>> GetAllAsync();
        Task<ToolCategory> GetByIdAsync(int id);
        Task <ToolCategory> AddAsync(ToolCategory category);
        Task UpdateAsync(ToolCategory category);
        Task DeleteAsync(int id);
    }
}
