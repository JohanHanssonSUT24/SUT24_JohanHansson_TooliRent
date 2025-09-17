using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooliRent.Application.DTOs;
using FluentValidation;

namespace TooliRent.Application.Validators
{
    public class CreateToolCategoryDtoValidator : AbstractValidator<CreateToolCategoryDto>
    {
        public CreateToolCategoryDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Category name is required");
        }

    }
}
