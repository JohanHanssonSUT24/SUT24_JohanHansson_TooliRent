using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TooliRent.Application.DTOs;
using TooliRent.Application.Interfaces.Services;
using TooliRent.Domain.Entities;
using TooliRent.Domain.Interfaces.Repositories; 

namespace TooliRent.Application.Services
{
    public class ToolService : IToolService
    {
        private readonly IToolRepository _toolRepository;
        private readonly IMapper _mapper;
        public ToolService(IToolRepository toolRepository, IMapper mapper)
        {
            _toolRepository = toolRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ToolDto>> GetAllToolsAsync()
        {
            var tools = await _toolRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ToolDto>>(tools);
        }
        public async Task<ToolDto> GetToolByIdAsync(int id)
        {
            var tool = await _toolRepository.GetByIdAsync(id);
            if (tool == null) return null;
            return _mapper.Map<ToolDto>(tool);
        }
        public async Task<ToolDto> CreateToolAsync(CreateToolDto toolDto)
        {
            var tool = _mapper.Map<Tool>(toolDto);
            await _toolRepository.AddAsync(tool);
            return _mapper.Map<ToolDto>(tool);
        }
        public async Task UpdateToolAsync(UpdateToolDto toolDto)
        {
            var toolToUpdate = await _toolRepository.GetByIdAsync(toolDto.Id);
            if (toolToUpdate == null) return;
            _mapper.Map(toolDto, toolToUpdate);
            await _toolRepository.UpdateAsync(toolToUpdate);
        }
        public async Task<bool> DeleteToolAsync(int id)
        {
            return await _toolRepository.DeleteAsync(id);

        }
    }
}
