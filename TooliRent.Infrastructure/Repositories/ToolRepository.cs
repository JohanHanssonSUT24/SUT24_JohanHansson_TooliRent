using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TooliRent.Domain.Entities;
using TooliRent.Domain.Enums;
using TooliRent.Domain.Interfaces.Repositories;
using TooliRent.Infrastructure.Data;

namespace TooliRent.Infrastructure.Repositories
{
    public class ToolRepository : IToolRepository
    {
        private readonly TooliRentDbContext _context;

        public ToolRepository(TooliRentDbContext context)
        {
            _context = context;
        }

        public async Task<Tool> GetByIdAsync(int id)
        {
            return await _context.Tools
                .Include(t => t.ToolCategory)
                .Where(t => !t.IsDeleted && t.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(Tool tool)
        {
            await _context.Tools.AddAsync(tool);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tool tool)
        {
            _context.Tools.Update(tool);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var toolToDelete = await _context.Tools.FindAsync(id);
            if (toolToDelete == null)
            {
                return false;
            }
            toolToDelete.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Tool>> GetToolsByFilterAsync(string? categoryName = null, string? status = null)
        {
            var query = _context.Tools.AsQueryable();

            query = query.Include(t => t.ToolCategory);

            if (!string.IsNullOrEmpty(categoryName))
            {
                query = query.Where(t => t.ToolCategory.Name.ToLower() == categoryName.ToLower());
            }

            if (!string.IsNullOrEmpty(status))
            {
                if (Enum.TryParse<ToolStatus>(status, true, out var toolStatus))
                {
                    query = query.Where(t => t.Status == toolStatus);
                }
            }

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<Tool>> GetAllAsync()
        {
            return await _context.Tools.ToListAsync();
        }
    }
}