using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task<IEnumerable<Tool>> GetAllAsync(string? searchTerm = null, ToolStatus? status = null)
        {
            var query = _context.Tools.AsQueryable().Where(t => !t.IsDeleted);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t => t.Name.Contains(searchTerm) || t.Description.Contains(searchTerm));
            }
            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }

            return await query.ToListAsync();
        }
        public async Task<Tool> GetByIdAsync(int id)
        {
            return await _context.Tools.Where(t=> !t.IsDeleted && t.Id == id).FirstOrDefaultAsync();
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
    }
}
