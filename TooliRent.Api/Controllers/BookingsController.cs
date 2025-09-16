using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TooliRent.Application.DTOs;
using TooliRent.Application.Interfaces.Services;
using System.Collections.Generic;
namespace TooliRent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Member")]
        public async Task<ActionResult<BookingDto>> GetBookingById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [HttpPost]
        [Authorize(Roles = "Member")]
        public async Task<ActionResult<BookingDto>> CreateBooking([FromBody] CreateBookingDto bookingDto)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null)
            {
                return Unauthorized();
            }

            if (!int.TryParse(userIdString, out int userId))
            {
                return BadRequest("Invalid user ID.");
            }

            var booking = await _bookingService.CreateBookingAsync(bookingDto, userId);
            if (booking == null)
            {
                return BadRequest("Could not create booking. The tool might not be available or the dates are invalid.");
            }
            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Member")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateBookingDto bookingDto)
        {
            var result = await _bookingService.UpdateBookingAsync(id, bookingDto);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpGet("mybookings")]
        [Authorize(Roles = "Member, Admin")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetMyBookings()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized(new {message = "User not found or invalid token."});
            }
            
            var bookings = await _bookingService.GetBookingsByUserIdAsync(userId);
            if(bookings == null ||  !bookings.Any())
            {
                return NotFound(new { message = "No bookings found for this user." });
            }
            return Ok(bookings);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Member, Admin")]
        public async Task<ActionResult> CancelBooking(int id)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized();
            }
            var result = await _bookingService.CancelBookingAsync(id, userId);
            if (result)
            {
                return Ok(new { message = "Booking cancelled successfully." });
            }
            return BadRequest(new {message = "Could not cancel booking. Booking not found."});
        }
    }
}
