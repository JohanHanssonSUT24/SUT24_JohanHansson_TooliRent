using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooliRent.Application.DTOs;
using TooliRent.Domain.Enums;

namespace TooliRent.Application.Interfaces.Services
{
    public interface IToolService
    {
        Task<IEnumerable<ToolDto>> GetAllToolsAsync(string? categoryName = null, string? status = null, int? categoryId = null);
        Task<ToolDto> GetToolByIdAsync(int id);
        Task<ToolDto> CreateToolAsync(CreateToolDto newToolDto);
        Task <bool>UpdateToolAsync(UpdateToolDto toolDto);
        Task<bool> DeleteToolAsync(int id);
    }
}
