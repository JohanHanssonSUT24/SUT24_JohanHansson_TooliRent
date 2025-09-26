using Microsoft.EntityFrameworkCore;
using TooliRent.Domain.Entities;
using TooliRent.Domain.Interfaces.Repositories;
using TooliRent.Infrastructure.Data;

namespace TooliRent.Infrastructure.Repositories
{
    public  class ToolCategoryRepository : IToolCategoryRepository
    {
        private readonly TooliRentDbContext _context;
        public ToolCategoryRepository(TooliRentDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ToolCategory>> GetAllAsync()//Retrieves all tool categories from the database.
        {
            return await _context.ToolCategories.ToListAsync();
        }
        public async Task<ToolCategory> GetByIdAsync(int id)//Retrieves a specific tool category by its ID from the database.
        {
            return await _context.ToolCategories.FindAsync(id);
        }

        public async Task<ToolCategory> AddAsync(ToolCategory category)//Adds a new tool category to the database and saves changes.
        {
            await _context.ToolCategories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task UpdateAsync(ToolCategory category)//Updates an existing tool category in the database and saves changes.
        {
            _context.ToolCategories.Update(category);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)//Deletes a tool category by its ID from the database and saves changes.
        {
            var category = await _context.ToolCategories.FindAsync(id);
            if (category != null)
            {
                _context.ToolCategories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
