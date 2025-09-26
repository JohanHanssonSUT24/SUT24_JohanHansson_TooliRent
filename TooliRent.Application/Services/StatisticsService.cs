using System.Linq;
using System.Threading.Tasks;
using TooliRent.Application.DTOs;
using TooliRent.Application.Interfaces.Services;
using TooliRent.Domain.Interfaces.Repositories;
using TooliRent.Domain.Enums;
using AutoMapper;

namespace TooliRent.Application.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IUserRepository _userRepository;
        private readonly IToolRepository _toolRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public StatisticsService(IUserRepository userRepository, IToolRepository toolRepository, IBookingRepository bookingRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _toolRepository = toolRepository;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }
        public async Task<StatisticsDto> GetStatisticsAsync()//Retrieves overall statistics including total users, tools, bookings, available tools, and rented tools.
        {
            var users = await _userRepository.GetAllAsync();
            var tools = await _toolRepository.GetAllAsync();
            var bookings = await _bookingRepository.GetAllBookingsAsync();

            var statistics = new StatisticsDto
            {
                TotalUsers = users.Count(),
                TotalTools = tools.Count(),
                TotalBookings = bookings.Count(),
                AvailableTools = tools.Count(t => t.Status == ToolStatus.Available),
                RentedTools = tools.Count(t => t.Status == ToolStatus.Rented)
            };
            return statistics;
        }
        public async Task<IEnumerable<ToolDto>> GetToolsByCategoryAsync(string categoryName)//Retrieves tools filtered by a specific category and maps them to ToolDto objects.
        {
            var tools = await _toolRepository.GetToolsByFilterAsync(categoryName);
            return _mapper.Map<IEnumerable<ToolDto>>(tools);
        }
        public async Task<ToolStatisticsDto> GetToolStatisticsByIdAsync(int toolId)//Retrieves statistics for a specific tool, including total rentals, and maps it to a ToolStatisticsDto object.
        {
            var tool = await _toolRepository.GetByIdAsync(toolId);
            if(tool == null)
            {
                return null;
            }
            var allBookings = await _bookingRepository.GetAllBookingsAsync();
            var totalRentals = allBookings.Count(b => b.ToolId == toolId);

            var toolDto = _mapper.Map<ToolStatisticsDto>(tool);
            toolDto.TotalRentals = totalRentals;

            return toolDto;
        }
    }
}
