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
    public class BookingService : IBookingService
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

        public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAllBookingsAsync();
            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }

        public async Task<BookingDto> GetBookingByIdAsync(int id)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            return _mapper.Map<BookingDto>(booking);
        }

        public async Task<BookingDto> CreateBookingAsync(CreateBookingDto bookingDto, int userId)
        {
            var tool = await _toolRepository.GetByIdAsync(bookingDto.ToolId);
            if (tool == null || tool.Status != ToolStatus.Available)
            {
                return null;
            }

            if (bookingDto.StartDate >= bookingDto.EndDate)
            {
                return null;
            }
            var overlappingBookings = await _bookingRepository.GetActiveBookingsForToolAsync(
                bookingDto.ToolId, 
                bookingDto.StartDate, 
                bookingDto.EndDate);
            if(overlappingBookings.Any())
            {
                return null;
            }

            var booking = _mapper.Map<Booking>(bookingDto);
            booking.UserId = userId;
            booking.TotalPrice = CalculateTotalPrice(bookingDto.StartDate, bookingDto.EndDate, tool.RentalPrice);
            booking.Status = BookingStatus.Active;

            await _bookingRepository.AddBookingAsync(booking);

            //tool.Status = ToolStatus.Rented;
            await _toolRepository.UpdateAsync(tool);

            return _mapper.Map<BookingDto>(booking);
        }

        private decimal CalculateTotalPrice(DateTime startDate, DateTime endDate, decimal rentalPrice)
        {
            var rentalDays = (endDate - startDate).TotalDays;
            return (decimal)rentalDays * rentalPrice;
        }

        public async Task<bool> UpdateBookingAsync(int id, UpdateBookingDto bookingDto)
        {
            var bookingToUpdate = await _bookingRepository.GetBookingByIdAsync(id);
            if (bookingToUpdate == null) return false;

            _mapper.Map(bookingDto, bookingToUpdate);

            // Specialfall: Om statusen ändras till Completed, uppdatera verktygsstatus
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

        public async Task<bool> DeleteBookingAsync(int id)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            if (booking == null) return false;

            // Om en aktiv bokning raderas, se till att verktyget blir tillgängligt igen
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
        public async Task<IEnumerable<BookingDto>> GetBookingsByUserIdAsync(int userId)
        {
            var bookings = await _bookingRepository.GetBookingsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }
        public async Task<bool> CancelBookingAsync(int bookingId, int userId)
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
    }
}
