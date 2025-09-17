using TooliRent.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TooliRent.Application.Interfaces.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetAllBookingsAsync();
        Task<BookingDto> GetBookingByIdAsync(int id);
        Task<BookingDto> CreateBookingAsync(CreateBookingDto bookingDto, int userId);
        Task<bool> UpdateBookingAsync(int id, UpdateBookingDto bookingDto);
        Task<IEnumerable<BookingDto>> GetBookingsByUserIdAsync(int userId);
        Task<bool> CancelBookingAsync(int bookingId, int userId);
        Task<bool> PickupBookingAsync(int bookingId, int userId);
        Task<bool> ReturnBookingAsync(int bookingId, int userId);
    }
}
