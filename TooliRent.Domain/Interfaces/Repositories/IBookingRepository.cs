using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooliRent.Domain.Entities;

namespace TooliRent.Domain.Interfaces.Repositories
{
    public interface IBookingRepository //Define Repository Interface to abstract data access logic and provide a contract for data operations.
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking> GetBookingByIdAsync(int id);
        Task AddBookingAsync(Booking booking);
        Task UpdateBookingAsync(Booking booking);
        Task DeleteBookingAsync(int id);

        Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId);
        Task<IEnumerable<Booking>> GetActiveBookingsForToolAsync(int toolId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Booking>> GetOverdueBookingsAsync();
    }
}
