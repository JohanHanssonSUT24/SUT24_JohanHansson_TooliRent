using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TooliRent.Application.Interfaces.Services;
using TooliRent.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace TooliRent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToolsController : ControllerBase
    {
        private readonly IToolService _toolsService;

        public ToolsController(IToolService toolsService)
        {
            _toolsService = toolsService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string searchTerm)
        {
            var tools = await _toolsService.GetAllToolsAsync(searchTerm);
            return Ok(tools);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tool = await _toolsService.GetToolByIdAsync(id);
            if (tool == null) return NotFound();
            return Ok(tool);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateToolDto newToolDto)
        {
            var createdTool = await _toolsService.CreateToolAsync(newToolDto);
            return CreatedAtAction(nameof(GetById), new { id = createdTool.Id }, createdTool);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateToolDto toolDto)
        {
            if (!ModelState.IsValid || id != toolDto.Id) return BadRequest(ModelState);

            await _toolsService.UpdateToolAsync(toolDto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _toolsService.DeleteToolAsync(id);
            return NoContent();
        }
    }
}
