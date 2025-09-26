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
        public async Task<ActionResult<StatisticsDto>> GetStatistics()// Get overall statistics
        {
            var statistics = await _statisticsService.GetStatisticsAsync();// Await the service to get statistics
            return Ok(statistics);
        }
        [HttpGet("category/{categoryName}")]
        public async Task<ActionResult<IEnumerable<ToolDto>>> GetToolsByCategory(string categoryName)// Get tools by category name
        {
            var tools = await _statisticsService.GetToolsByCategoryAsync(categoryName);// Await the service to get tools by category
            if (tools == null || !tools.Any())
            {
                return NotFound($"No tools found in category {categoryName}.");
            }
            return Ok(tools);
        }
        [HttpGet("tool/{toolId}")]
        public async Task<ActionResult<ToolStatisticsDto>> GetToolStatistics(int toolId)// Get statistics for a specific tool by ID
        {
            var statistics = await _statisticsService.GetToolStatisticsByIdAsync(toolId);// Await the service to get tool statistics by ID
            if (statistics == null)
            {
                return NotFound($"No tool with ID '{toolId}' found.");
            }
            return Ok(statistics);
        }
    }
}
