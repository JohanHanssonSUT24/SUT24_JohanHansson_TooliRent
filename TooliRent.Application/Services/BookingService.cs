using System;
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
    public class BookingService : IBookingService //Create Service and implement Interface to handle business logic related to that service.
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IToolRepository _toolRepository;
        private readonly IMapper _mapper;

        public BookingService(IBookingRepository bookingRepository, IToolRepository toolRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _toolRepository = toolRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()//Retrieves all bookings from the repository and maps them to BookingDto objects.
        {
            var bookings = await _bookingRepository.GetAllBookingsAsync();// Await the repository to get all bookings
            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }

        public async Task<BookingDto> GetBookingByIdAsync(int id)//Retrieves a specific booking by its ID and maps it to a BookingDto object.
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(id);// Await the repository to get booking by id
            return _mapper.Map<BookingDto>(booking);
        }

        public async Task<BookingDto> CreateBookingAsync(CreateBookingDto bookingDto, int userId)//Creates a new booking after validating tool availability and booking dates.
        {
            var tool = await _toolRepository.GetByIdAsync(bookingDto.ToolId);// Await the repository to get tool by id
            if (tool == null || tool.Status != ToolStatus.Available) // Check if tool exists and is available
            {
                return null;
            }

            if (bookingDto.StartDate >= bookingDto.EndDate)// Validate booking dates
            {
                return null;
            }
            var overlappingBookings = await _bookingRepository.GetActiveBookingsForToolAsync(
                bookingDto.ToolId, 
                bookingDto.StartDate, 
                bookingDto.EndDate);// Check for overlapping bookings
            if (overlappingBookings.Any())
            {
                return null;
            }

            var booking = _mapper.Map<Booking>(bookingDto);// Map DTO to entity
            booking.UserId = userId;
            booking.TotalPrice = CalculateTotalPrice(bookingDto.StartDate, bookingDto.EndDate, tool.RentalPrice);
            booking.Status = BookingStatus.Active;

            await _bookingRepository.AddBookingAsync(booking);

            tool.Status = ToolStatus.Rented;
            await _toolRepository.UpdateAsync(tool);

            return _mapper.Map<BookingDto>(booking);
        }

        private decimal CalculateTotalPrice(DateTime startDate, DateTime endDate, decimal rentalPrice)//Calculates the total price of a booking based on the rental duration and tool's rental price.
        {
            var rentalDays = (endDate - startDate).TotalDays;
            return (decimal)rentalDays * rentalPrice;
        }

        public async Task<bool> UpdateBookingAsync(int id, UpdateBookingDto bookingDto)//Updates an existing booking and handles tool status changes if the booking is completed.
        {
            var bookingToUpdate = await _bookingRepository.GetBookingByIdAsync(id);
            if (bookingToUpdate == null) return false;

            _mapper.Map(bookingDto, bookingToUpdate);

            if (bookingDto.Status == BookingStatus.Completed)
            {
                var tool = await _toolRepository.GetByIdAsync(bookingToUpdate.ToolId);
                if (tool != null)
                {
                    tool.Status = ToolStatus.Available;
                    await _toolRepository.UpdateAsync(tool);
                }
            }

            await _bookingRepository.UpdateBookingAsync(bookingToUpdate);
            return true;
        }

        public async Task<bool> DeleteBookingAsync(int id) //Deletes a booking and updates the tool status if the booking was active.
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            if (booking == null) return false;

            if (booking.Status == BookingStatus.Active)
            {
                var tool = await _toolRepository.GetByIdAsync(booking.ToolId);
                if (tool != null)
                {
                    tool.Status = ToolStatus.Available;
                    await _toolRepository.UpdateAsync(tool);
                }
            }

            await _bookingRepository.DeleteBookingAsync(id);
            return true;
        }
        public async Task<IEnumerable<BookingDto>> GetBookingsByUserIdAsync(int userId)//Retrieves all bookings associated with a specific user and maps them to BookingDto objects.
        {
            var bookings = await _bookingRepository.GetBookingsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }
        public async Task<bool> CancelBookingAsync(int bookingId, int userId)//Cancels a booking if it belongs to the user and updates the tool status to available.
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
            if (booking == null || booking.UserId != userId)
            {
                return false;
            }
            booking.Status = BookingStatus.Cancelled;
            await _bookingRepository.UpdateBookingAsync(booking);

            var tool = await _toolRepository.GetByIdAsync(booking.ToolId);
            if( tool != null)
            {
                tool.Status = ToolStatus.Available;
                await _toolRepository.UpdateAsync(tool);
            }
            return true;
        }
        public async Task<bool> PickupBookingAsync(int bookingId, int userId)//Marks a booking as picked up if it belongs to the user and is in an appropriate status, updating the tool status to rented.
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);

            if (booking == null)
            {
                return false;
            }

            if (booking.UserId != userId || (booking.Status != BookingStatus.Active && booking.Status != BookingStatus.Approved))
            {
                return false;
            }

            booking.Status = BookingStatus.PickedUp;
            await _bookingRepository.UpdateBookingAsync(booking);

            var tool = await _toolRepository.GetByIdAsync(booking.ToolId);
            if (tool != null)
            {
                tool.Status = ToolStatus.Rented;
                await _toolRepository.UpdateAsync(tool);
            }
            return true;
        }
        public async Task<bool> ReturnBookingAsync(int bookingId, int userId)//Marks a booking as completed if it belongs to the user and is currently picked up, updating the tool status to available.
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
            if (booking == null)
            {
                return false;
            }
            if (booking.UserId != userId || booking.Status != BookingStatus.PickedUp)
            {
                return false;
            }
            booking.Status = BookingStatus.Completed;
            await _bookingRepository.UpdateBookingAsync(booking);

            var tool = await _toolRepository.GetByIdAsync(booking.ToolId);
            if (tool != null)
            {
                tool.Status = ToolStatus.Available;
                await _toolRepository.UpdateAsync(tool);
            }
            return true;
        }
        public async Task<bool> MarkBookingAsOverdueAsync(int bookingId)//Marks a booking as overdue if it is currently picked up and the end date has passed.
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
            if(booking == null || booking.Status != BookingStatus.PickedUp || booking.EndDate >= DateTime.UtcNow)
            {
                return false;
            }
            booking.Status = BookingStatus.Overdue;
            await _bookingRepository.UpdateBookingAsync(booking);
            return true;
        }
        public async Task<IEnumerable<BookingDto>> GetOverdueBookingsAsync()//Retrieves all overdue bookings and maps them to BookingDto objects.
        {
            var bookings = await _bookingRepository.GetOverdueBookingsAsync();
            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }
        public async Task<bool> ApprovedBookingAsync(int bookingId)//Approves a pending or active booking, changing its status to approved.
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
            if (booking == null || (booking.Status != BookingStatus.Pending && booking.Status != BookingStatus.Active))
            {
                return false;
            }
            booking.Status = BookingStatus.Approved;
            await _bookingRepository.UpdateBookingAsync(booking);
            return true;
        }
    }
}
