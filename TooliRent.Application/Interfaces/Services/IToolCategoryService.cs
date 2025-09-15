using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooliRent.Application.DTOs;

namespace TooliRent.Application.Interfaces.Services
{
    public interface IToolCategoryService
    {
        Task<IEnumerable<ToolCategoryDto>> GetAllCategoriesAsync();
        Task<ToolCategoryDto> GetCategoryByIdAsync(int id);
        Task<ToolCategoryDto> AddCategoryAsync(CreateToolCategoryDto categoryDto);
        Task<bool> UpdateCategoryAsync(int id, UpdateToolCategoryDto categoryDto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
