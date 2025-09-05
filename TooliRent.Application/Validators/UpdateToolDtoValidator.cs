using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TooliRent.Application.DTOs;

namespace TooliRent.Application.Validators
{
    public class UpdateToolDtoValidator : AbstractValidator<UpdateToolDto>
    {
        public UpdateToolDtoValidator() 
        {
            RuleFor(tool => tool.Id).NotEmpty().WithMessage("Tool ID is required for update.");

            RuleFor(tool => tool.Name).NotEmpty().WithMessage("Tool name is required.")
                                      .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");

            RuleFor(tool => tool.Description).NotEmpty().WithMessage("Tool description is required.");

            RuleFor(tool => tool.RentalPrice).NotEmpty().WithMessage("Rental price must be greater than 0.");
        }
    }
}
