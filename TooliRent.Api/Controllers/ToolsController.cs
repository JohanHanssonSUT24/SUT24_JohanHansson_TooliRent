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
        [AllowAnonymous]// Allow anonymous access to this endpoint
        public async Task<IActionResult> GetAll([FromQuery] string? searchTerm = null, [FromQuery] ToolStatus? status = null, [FromQuery] int? categoryId = null)// Get all tools with optional filtering
        {
            var statusString = status?.ToString();// Convert enum to string if not null
            var tools = await _toolsService.GetAllToolsAsync(searchTerm, statusString, categoryId);// Await the service to get all tools with optional filters
            return Ok(tools);
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)// Get tool by id
        {
            var tool = await _toolsService.GetToolByIdAsync(id);// Await the service to get tool by id
            if (tool == null) return NotFound();
            return Ok(tool);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateToolDto newToolDto)// Create new tool
        {
            var createdTool = await _toolsService.CreateToolAsync(newToolDto);// Call service to create new tool
            return CreatedAtAction(nameof(GetById), new { id = createdTool.Id }, createdTool);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateToolDto toolDto)// Update existing tool
        {
            if (id != toolDto.Id)// Check if id in URL matches id in body
            {
                return BadRequest("Tool ID in URL and body do not match");
            }

            var updateSuccessful = await _toolsService.UpdateToolAsync(toolDto);// Call service to update tool
            if (!updateSuccessful)
            {
                return NotFound();
            }

            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)// Delete tool by id
        {
            await _toolsService.DeleteToolAsync(id);// Call service to delete tool
            return NoContent();
        }
    }
}
