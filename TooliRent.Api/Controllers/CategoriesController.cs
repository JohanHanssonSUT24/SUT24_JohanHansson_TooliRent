using Microsoft.AspNetCore.Mvc;
using TooliRent.Application.DTOs;
using TooliRent.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace TooliRent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IToolCategoryService _categoryService;

        public CategoriesController(IToolCategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GettAll()// Get all categories
        {
            var categories = await _categoryService.GetAllCategoriesAsync();// Await the service to get all categories
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)// Get category by id
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);// Await the service to get category by id
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] CreateToolCategoryDto categoryDto)// Add new category
        {
            var createdCategory = await _categoryService.AddCategoryAsync(categoryDto);// Call service to add new category
            return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);// Return 201 Created response
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateToolCategoryDto categoryDto)// Update existing category
        {
            if (id != categoryDto.Id)// Check if id in URL matches id in body
            {
                return BadRequest("Category ID in URL and body do not match.");
            }
            var result = await _categoryService.UpdateCategoryAsync(id, categoryDto);// Call service to update category
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task <IActionResult> Delete(int id)// Delete category by id
        {
            var resutl = await _categoryService.DeleteCategoryAsync(id);// Call service to delete category
            if (!resutl)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
