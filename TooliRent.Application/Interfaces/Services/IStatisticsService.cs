using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooliRent.Application.DTOs;

namespace TooliRent.Application.Interfaces.Services
{
    public interface IStatisticsService
    {
        Task<StatisticsDto> GetStatisticsAsync();
        Task<IEnumerable<ToolDto>> GetToolsByCategoryAsync(string categoryName);
        Task<ToolStatisticsDto> GetToolStatisticsByIdAsync(int toolId);
    }
}
