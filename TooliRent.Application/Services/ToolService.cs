using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TooliRent.Application.DTOs;
using TooliRent.Application.Interfaces.Services;
using TooliRent.Domain.Entities;
using TooliRent.Domain.Enums;
using TooliRent.Domain.Interfaces.Repositories;

namespace TooliRent.Application.Services
{
    public class ToolService : IToolService
    {
        private readonly IToolRepository _toolRepository;
        private readonly IMapper _mapper;
        private readonly IToolCategoryRepository _toolCategoryRepository;

        public ToolService(IToolRepository toolRepository, IMapper mapper, IToolCategoryRepository categoryRepository)
        {
            _toolRepository = toolRepository;
            _mapper = mapper;
            _toolCategoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<ToolDto>> GetAllToolsAsync(string? categoryName = null, string? status = null, int? categoryId = null)
        {
            var tools = await _toolRepository.GetToolsByFilterAsync(categoryName, status, categoryId);
            return _mapper.Map<IEnumerable<ToolDto>>(tools);
        }

        public async Task<ToolDto> GetToolByIdAsync(int id)
        {
            var tool = await _toolRepository.GetByIdAsync(id);
            if (tool == null) return null;
            return _mapper.Map<ToolDto>(tool);
        }

        public async Task<ToolDto> CreateToolAsync(CreateToolDto newToolDto)
        {
            var tool = _mapper.Map<Tool>(newToolDto);
            if (newToolDto.ToolCategoryId.HasValue)
            {
                var category = await _toolCategoryRepository.GetByIdAsync(newToolDto.ToolCategoryId.Value);
                if(category != null)
                {
                    tool.ToolCategory = category;
                }
            }
            await _toolRepository.AddAsync(tool);
            return _mapper.Map<ToolDto>(tool);
        }

        public async Task<bool> UpdateToolAsync(UpdateToolDto toolDto)
        {
            var toolToUpdate = await _toolRepository.GetByIdAsync(toolDto.Id);
            if (toolToUpdate == null)
            {
                return false;
            }
            _mapper.Map(toolDto, toolToUpdate);
            await _toolRepository.UpdateAsync(toolToUpdate);
            return true;
        }

        public async Task<bool> DeleteToolAsync(int id)
        {
            return await _toolRepository.DeleteAsync(id);
        }
    }
}