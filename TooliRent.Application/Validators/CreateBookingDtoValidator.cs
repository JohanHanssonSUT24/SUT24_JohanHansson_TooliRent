using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TooliRent.Application.DTOs;

namespace TooliRent.Application.Validators
{
    public class CreateBookingDtoValidator : AbstractValidator<CreateBookingDto>// Create validators for DTOs using FluentValidation.Set up rules for specific validation scenarios.
    {
        public CreateBookingDtoValidator()
        {
            RuleFor(dto => dto.ToolId)
                .GreaterThan(0)
                .WithMessage("ToolId must be greater than 0.");
            RuleFor(dto => dto.StartDate)
                .NotEmpty()
                .WithMessage("StartDate cant be left blank.")
                .Must(BeAValidFutureDate)
                .WithMessage("StartDate must be a future date.");

            RuleFor(dto => dto.EndDate)
                .NotEmpty()
                .WithMessage("EndDate cant be left blank.")
                .Must(BeAValidFutureDate)
                .WithMessage("EndDate must be after StartDate.")
                .GreaterThan(dto => dto.StartDate)
                .WithMessage("EndDate must be after StartDate.");
        }
        private bool BeAValidFutureDate(DateTime date)
        {
            return date.Date > DateTime.Today;
        }
    }
}
