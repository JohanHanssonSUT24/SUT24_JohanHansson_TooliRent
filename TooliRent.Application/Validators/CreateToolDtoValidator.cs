using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TooliRent.Application.DTOs;

namespace TooliRent.Application.Validators
{
    public class CreateToolDtoValidator : AbstractValidator<CreateToolDto>
    {
        public CreateToolDtoValidator()
        {
            RuleFor(tool => tool.Name)
                .NotEmpty().WithMessage("Tool name is required.")
                .Length(2, 50).WithMessage("Name must  be between 2 and 50 characters.");
            RuleFor(tool => tool.Description)
                .NotEmpty().WithMessage("Description is required");
            RuleFor(tool => tool.RentalPrice)
                .GreaterThan(0).WithMessage("Rental price must be greater than zero.");
        }
    }
}
