using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TooliRent.Domain.Entities;
using TooliRent.Domain.Interfaces.Repositories;
using TooliRent.Infrastructure.Data;

namespace TooliRent.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly TooliRentDbContext _context;
        public BookingRepository(TooliRentDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings.Include(b => b.Tool).Include(b => b.User).ToListAsync();
        }
        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings.Include(b => b.Tool).Include(b => b.User).FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task AddBookingAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookingAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId)
        {
            return await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Tool)
                .Include(b => b.User)
                .ToListAsync();
        }
        public async Task<IEnumerable<Booking>> GetActiveBookingsForToolAsync(int toolId, DateTime startDate, DateTime endDate)
        {
            return await _context.Bookings
                .Where(b => b.ToolId == toolId &&
                b.Status == Domain.Enums.BookingStatus.Active &&
                (startDate < b.EndDate && endDate > b.StartDate))

                .ToListAsync();
        }
    }
}
