using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TooliRent.Domain.Entities;
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
        public async Task<IEnumerable<Tool>> GetAllAsync()
        {
            return await _context.Tools.ToListAsync();
        }
        public async Task<Tool> GetByIdAsync(int id)
        {
            return await _context.Tools.FindAsync(id);
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
        public async Task DeleteAsync(int id)
        {
            var toolToDelete = await _context.Tools.FindAsync(id);
            if (toolToDelete != null)
            {
                _context.Tools.Remove(toolToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
