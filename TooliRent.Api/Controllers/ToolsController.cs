using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TooliRent.Application.Interfaces.Services;
using TooliRent.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using TooliRent.Domain.Enums;

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
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] string? searchTerm = null, [FromQuery] ToolStatus? status = null)
        {
            var tools = await _toolsService.GetAllToolsAsync(searchTerm, status);
            return Ok(tools);
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var tool = await _toolsService.GetToolByIdAsync(id);
            if (tool == null) return NotFound();
            return Ok(tool);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateToolDto newToolDto)
        {
            var createdTool = await _toolsService.CreateToolAsync(newToolDto);
            return CreatedAtAction(nameof(GetById), new { id = createdTool.Id }, createdTool);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateToolDto toolDto)
        {
            if (id != toolDto.Id)
            {
                return BadRequest("Tool ID in URL and body do not match");
            }

            var updateSuccessful = await _toolsService.UpdateToolAsync(toolDto);
            if(!updateSuccessful)
            {
                return NotFound();
            }

            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _toolsService.DeleteToolAsync(id);
            return NoContent();
        }
    }
}
