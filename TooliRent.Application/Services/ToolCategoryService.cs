using AutoMapper;
using TooliRent.Application.DTOs;
using TooliRent.Application.Interfaces.Services;
using TooliRent.Domain.Entities;
using TooliRent.Domain.Interfaces.Repositories;

namespace TooliRent.Application.Services
{
    public class ToolCategoryService : IToolCategoryService
    {
        private readonly IToolCategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ToolCategoryService(IToolCategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ToolCategoryDto>> GetAllCategoriesAsync()//Retrieves all tool categories from the repository and maps them to ToolCategoryDto objects.
        {
            var categories = await _categoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ToolCategoryDto>>(categories);
        }
        public async Task<ToolCategoryDto> GetCategoryByIdAsync(int id)//Retrieves a specific tool category by its ID and maps it to a ToolCategoryDto object.
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return _mapper.Map<ToolCategoryDto>(category);
        }
        public async Task<ToolCategoryDto> AddCategoryAsync(CreateToolCategoryDto categoryDto)//Adds a new tool category to the repository after mapping it from a CreateToolCategoryDto object.
        {
            var category = _mapper.Map<ToolCategory>(categoryDto);
            await _categoryRepository.AddAsync(category);
            return _mapper.Map<ToolCategoryDto>(category);
        }
        public async Task<bool> UpdateCategoryAsync(int id, UpdateToolCategoryDto categoryDto)//Updates an existing tool category by its ID after mapping the updated data from an UpdateToolCategoryDto object.
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null) return false;

            _mapper.Map(categoryDto, existingCategory);
            await _categoryRepository.UpdateAsync(existingCategory);
            return true;
        }
        public async Task<bool> DeleteCategoryAsync(int id)//Deletes a tool category by its ID from the repository.
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return false;
            await _categoryRepository.DeleteAsync(id);
            return true;
        }
    }
}
