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

        public async Task<Tool> GetByIdAsync(int id)//Retrieves a specific tool by its ID, including its category, from the database if it is not marked as deleted.
        {
            return await _context.Tools
                .Include(t => t.ToolCategory)
                .Where(t => !t.IsDeleted && t.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(Tool tool)//Adds a new tool to the database and saves changes.
        {
            await _context.Tools.AddAsync(tool);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tool tool)//Updates an existing tool in the database and saves changes.
        {
            _context.Tools.Update(tool);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)//Soft-deletes a tool by its ID by setting its IsDeleted flag to true and saves changes.
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

        public async Task<IEnumerable<Tool>> GetToolsByFilterAsync(string? categoryName = null, string? status = null, int? categoryId = null)//Retrieves tools from the database with optional filtering by category name, status, or category ID, including their categories.
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
            if(categoryId.HasValue)
            {
                query = query.Where(t => t.ToolCategoryId == categoryId.Value);
            }

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<Tool>> GetAllAsync()//Retrieves all tools from the database.
        {
            return await _context.Tools.ToListAsync();
        }
    }
}