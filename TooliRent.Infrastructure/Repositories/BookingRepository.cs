using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TooliRent.Domain.Entities;
using TooliRent.Domain.Interfaces.Repositories;
using TooliRent.Infrastructure.Data;
using TooliRent.Domain.Enums;

namespace TooliRent.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository //Implement Repository to handle data access logic and interact with the database using EF.
    {
        private readonly TooliRentDbContext _context;
        public BookingRepository(TooliRentDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Booking>> GetAllBookingsAsync() //Retrieve all bookings, including related Tool and User entities, from the database.
        {
            return await _context.Bookings.Include(b => b.Tool).Include(b => b.User).ToListAsync();// Include related entities
        }
        public async Task<Booking> GetBookingByIdAsync(int id)//Retrieve a specific booking by its ID, including related Tool and User entities, from the database.
        {
            return await _context.Bookings.Include(b => b.Tool).Include(b => b.User).FirstOrDefaultAsync(b => b.Id == id);// Include related entities
        }
        public async Task AddBookingAsync(Booking booking)//Add a new booking to the database and save changes.
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookingAsync(Booking booking)//Update an existing booking in the database and save changes.
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookingAsync(int id)//Delete a booking by its ID from the database and save changes.
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId)//Retrieve all bookings associated with a specific user, including related Tool and User entities, from the database.
        {
            return await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Tool)
                .Include(b => b.User)
                .ToListAsync();
        }
        public async Task<IEnumerable<Booking>> GetActiveBookingsForToolAsync(int toolId, DateTime startDate, DateTime endDate)//Retrieve active bookings for a specific tool that overlap with a given date range from the database.
        {
            return await _context.Bookings
                .Where(b => b.ToolId == toolId &&
                b.Status == Domain.Enums.BookingStatus.Active &&
                (startDate < b.EndDate && endDate > b.StartDate))

                .ToListAsync();
        }
        public async Task<IEnumerable<Booking>> GetOverdueBookingsAsync()//Retrieve all bookings that are overdue (i.e., picked up but not returned by the end date), including related Tool and User entities, from the database.
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Tool)
                .Where(b => b.Status == BookingStatus.PickedUp && b.EndDate < DateTime.UtcNow)
                .ToListAsync();
        }
    }
}
