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
        public async Task<IEnumerable<ToolCategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ToolCategoryDto>>(categories);
        }
        public async Task<ToolCategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return _mapper.Map<ToolCategoryDto>(category);
        }
        public async Task<ToolCategoryDto> AddCategoryAsync(CreateToolCategoryDto categoryDto)
        {
            var category = _mapper.Map<ToolCategory>(categoryDto);
            await _categoryRepository.AddAsync(category);
            return _mapper.Map<ToolCategoryDto>(category);
        }
        public async Task<bool> UpdateCategoryAsync(int id, UpdateToolCategoryDto categoryDto)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null) return false;

            _mapper.Map(categoryDto, existingCategory);
            await _categoryRepository.UpdateAsync(existingCategory);
            return true;
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return false;
            await _categoryRepository.DeleteAsync(id);
            return true;
        }
    }
}
