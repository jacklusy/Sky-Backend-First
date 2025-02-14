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
                .NotEmpty().WithMessage("Employee number is required")
                .MaximumLength(6).WithMessage("Employee number cannot exceed 6 characters");

            RuleFor(x => x.EmployeeName)
                .NotEmpty().WithMessage("Employee name is required")
                .MaximumLength(100).WithMessage("Employee name cannot exceed 100 characters");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required")
                .Must(gender => gender.Length >= 1).WithMessage("Gender must be provided");

            RuleFor(x => x.DepartmentName)
                .NotEmpty().WithMessage("Department name is required");

            RuleFor(x => x.PositionName)
                .NotEmpty().WithMessage("Position name is required");

            RuleFor(x => x.VacationDaysLeft)
                .InclusiveBetween(0, 24)
                .WithMessage("Vacation days must be between 0 and 24");

            RuleFor(x => x.Salary)
                .GreaterThan(0).WithMessage("Salary must be greater than 0")
                .PrecisionScale(18, 2, true)
                .WithMessage("Salary must be positive with maximum 2 decimal places");
        }
    }
}