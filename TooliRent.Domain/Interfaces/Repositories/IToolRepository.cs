using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooliRent.Domain.Entities;
using TooliRent.Domain.Enums;

namespace TooliRent.Domain.Interfaces.Repositories
{
    public interface IToolRepository
    {
        Task<Tool> GetByIdAsync(int id);
        Task AddAsync(Tool tool);
        Task UpdateAsync(Tool tool);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Tool>> GetAllAsync();
        Task<IEnumerable<Tool>> GetToolsByFilterAsync(string? categoryName, string? status = null, int? categoryId = null);
    }
}
