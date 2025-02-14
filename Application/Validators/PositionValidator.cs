using FluentValidation;
using EmployeeManagement.Application.DTOs;

namespace EmployeeManagement.Application.Validators
{
    public class PositionValidator : AbstractValidator<PositionDto>
    {
        public PositionValidator()
        {
            RuleFor(x => x.PositionName)
                .NotEmpty()
                .MaximumLength(30)
                .WithMessage("Position name must not exceed 30 characters");
        }
    }
} 