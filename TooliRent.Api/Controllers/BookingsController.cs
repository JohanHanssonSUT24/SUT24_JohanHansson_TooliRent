using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TooliRent.Application.DTOs;
using TooliRent.Application.Interfaces.Services;
using System.Collections.Generic;
namespace TooliRent.Api.Controllers
{
    [Route("api/[controller]")] //Set base route to api/*
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService; // Dependency injection for business logic

        public BookingsController(IBookingService bookingService) //Constructor for DI
        {
            _bookingService = bookingService; 
        }

        //Endpoints
        [HttpGet]
        [Authorize(Roles = "Admin")]// Only users with Admin role can access this endpoint
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();// Await the service to get all bookings
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Member")] // Only users with Admin or Member role can access this endpoint
        public async Task<ActionResult<BookingDto>> GetBookingById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id); // Await the service to get booking by id
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [HttpPost]
        [Authorize(Roles = "Member, Admin")]
        public async Task<ActionResult<BookingDto>> CreateBooking([FromBody] CreateBookingDto bookingDto) //Returns a Task of ActionResult, expecting a CreateBookingDto in the request body
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;// Retrive user ID from JWT token
            if (userIdString == null) //Check if Null
            {
                return Unauthorized();
            }

            if (!int.TryParse(userIdString, out int userId))//TryParse to Int. If fail reurn 400 bad request respons.
            {
                return BadRequest("Invalid user ID.");
            }

            var booking = await _bookingService.CreateBookingAsync(bookingDto, userId); //Call service to create new booking
            if (booking == null)
            {
                return BadRequest("Could not create booking. The tool might not be available or the dates are invalid.");
            }
            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Member")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateBookingDto bookingDto) //Returns a Task of IActionResult, expecting an UpdateBookingDto in the request body
        {
            var result = await _bookingService.UpdateBookingAsync(id, bookingDto); //Call service to update booking
            if (!result)
            {
                return NotFound(); // If booking not found, return 404 Not Found
            }
            return NoContent();// If successful, return 204 No Content
        }
        [HttpGet("mybookings")]
        [Authorize(Roles = "Member, Admin")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetMyBookings() // Get bookings for the authenticated user
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized(new {message = "User not found or invalid token."});
            }
            
            var bookings = await _bookingService.GetBookingsByUserIdAsync(userId); // Call service to get bookings by user ID
            if (bookings == null ||  !bookings.Any())
            {
                return NotFound(new { message = "No bookings found for this user." });// If no bookings found, return 404 Not Found
            }
            return Ok(bookings);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Member, Admin")]
        public async Task<ActionResult> CancelBooking(int id)// Cancel a booking by ID
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;// Get user ID from JWT token
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))// Check if user ID is valid
            {
                return Unauthorized();
            }
            var result = await _bookingService.CancelBookingAsync(id, userId);// Call service to cancel booking
            if (result)
            {
                return Ok(new { message = "Booking cancelled successfully." });//Verify if successful
            }
            return BadRequest(new {message = "Could not cancel booking. Booking not found."});// If not successful, return 400 Bad Request
        }
        [HttpPost("{id}/pickup")]
        [Authorize(Roles = "Member,Admin")]
        public async Task<ActionResult> PickupBooking(int id)// Mark a booking as picked up
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))// Check if user ID is valid
            {
                return Unauthorized();
            }
            var result = await _bookingService.PickupBookingAsync(id, userId);// Call service to pick up booking
            if (result)
            {
                return Ok(new { message = "Booking picked up successfully." });
            }
            return BadRequest(new { message = "Could not pick up booking. Booking not found or invalid status." });
        }
        [HttpPost("{id}/return")]
        [Authorize(Roles = "Member,Admin")]
        public async Task<ActionResult> ReturnBooking(int id)// Mark a booking as returned
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))// Check if user ID is valid
            {
                return Unauthorized();
            }
            var result = await _bookingService.ReturnBookingAsync(id, userId);// Call service to return booking
            if (result)
            {
                return Ok(new { message = "Booking returned successfully." });
            }
            return BadRequest(new { message = "Could not return booking. Booking not found or invalid status." });
        }
        [HttpGet("overdue")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetOverdueBookings()// Get all overdue bookings
        {
            var bookings = await _bookingService.GetOverdueBookingsAsync();
            if (bookings == null || !bookings.Any())// If no overdue bookings found, return 404 Not Found
            {
                return NotFound(new { message = "No overdue bookings found." });
            }
            return Ok(bookings);
        }
        [HttpPost("{id}/markoverdue")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> MarkBookingAsOverdue(int id)// Mark a booking as overdue
        {
            var result = await _bookingService.MarkBookingAsOverdueAsync(id);// Call service to mark booking as overdue
            if (result)
            {
                return Ok(new { message = "Booking marked as overdue successfully." });
            }
            return BadRequest(new { message = "Could not mark booking as overdue. Booking not found or invalid status." });
        }
        [HttpPost("{id}/approved")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApprovedBooking(int id)// Approve a booking
        {
            var result = await _bookingService.ApprovedBookingAsync(id);// Call service to approve booking
            if (result)
            {
                return Ok(new { message = "Booking approved successfully." });
            }
            return BadRequest(new { message = "Could not approve booking. Booking not found or invalid status." });
        }
    }
}
