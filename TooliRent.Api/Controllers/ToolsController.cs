using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TooliRent.Application.Interfaces.Services;
using TooliRent.Application.DTOs;

namespace TooliRent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToolsController : ControllerBase
    {
        private readonly IToolsService _toolsService;

        public ToolsController(IToolsService toolsService)
        {
            _toolsService = toolsService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tools = await _toolsService.GetAllToolsAsync();
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
        public async Task<IActionResult> Update(int id, UpdateToolDto toolDto)
        {
            if (id != toolDto.Id) return BadRequest();

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
