using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooliRent.Application.DTOs;

namespace TooliRent.Application.Interfaces.Services
{
    public interface IToolService
    {
        Task<IEnumerable<ToolDto>> GetAllToolsAsync();
        Task<ToolDto> GetToolByIdAsync(int id);
        Task<ToolDto> CreateToolAsync(CreateToolDto toolDto);
        Task UpdateToolAsync(UpdateToolDto toolDto);
        Task DeleteToolAsync(int id);
    }
}
