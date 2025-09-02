using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TooliRent.Application.DTOs;
using TooliRent.Application.Interfaces.Services;
using TooliRent.Domain.Interfaces.Repositories; 

namespace TooliRent.Application.Services
{
    public class ToolService : IToolsService
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
    }
}
