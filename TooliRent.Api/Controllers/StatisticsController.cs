using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TooliRent.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using TooliRent.Application.DTOs;

namespace TooliRent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;
        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }
        [HttpGet]
        public async Task<ActionResult<StatisticsDto>> GetStatistics()
        {
            var statistics = await _statisticsService.GetStatisticsAsync();
            return Ok(statistics);
        }
        [HttpGet("category/{categoryName}")]
        public async Task<ActionResult<IEnumerable<ToolDto>>> GetToolsByCategory(string categoryName)
        {
            var tools = await _statisticsService.GetToolsByCategoryAsync(categoryName);
            if(tools == null || !tools.Any())
            {
                return NotFound($"No tools found in category {categoryName}.");
            }
            return Ok(tools);
        }
        [HttpGet("tool/{toolId}")]
        public async Task<ActionResult<ToolStatisticsDto>> GetToolStatistics(int toolId)
        {
            var statistics = await _statisticsService.GetToolStatisticsByIdAsync(toolId);
            if(statistics == null)
            {
                return NotFound($"No tool with ID '{toolId}' found.");
            }
            return Ok(statistics);
        }
    }
}
