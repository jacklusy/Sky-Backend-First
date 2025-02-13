using AutoMapper;
using FluentValidation;
using EmployeeManagement.Domain.Exceptions;

namespace EmployeeManagement.Application.Validators
{
    public class EmployeeValidator : AbstractValidator<EmployeeDto>
    {
        public EmployeeValidator()
        {
            RuleFor(x => x.EmployeeNumber)
                .NotEmpty()
                .MaximumLength(6)
                .Matches("^[A-Z0-9]+$")
                .WithMessage("Employee number must be up to 6 alphanumeric characters");

            RuleFor(x => x.EmployeeName)
                .NotEmpty()
                .MaximumLength(20)
                .WithMessage("Employee name must not exceed 20 characters");

            RuleFor(x => x.Gender)
                .NotEmpty()
                .Must(x => x == "M" || x == "F")
                .WithMessage("Gender must be either 'M' or 'F'");

            RuleFor(x => x.VacationDaysLeft)
                .InclusiveBetween(0, 24)
                .WithMessage("Vacation days must be between 0 and 24");

            RuleFor(x => x.Salary)
                .GreaterThan(0)
                .PrecisionScale(18, 2, true)
                .WithMessage("Salary must be positive with maximum 2 decimal places");
        }
    }
}