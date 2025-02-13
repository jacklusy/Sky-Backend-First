using FluentValidation;

namespace EmployeeManagement.Application.Validators
{
    public class VacationRequestValidator : AbstractValidator<VacationRequestDto>
    {
        public VacationRequestValidator()
        {
            RuleFor(x => x.EmployeeNumber)
                .NotEmpty()
                .MaximumLength(6)
                .WithMessage("Employee number is required and must not exceed 6 characters");

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Description is required and must not exceed 100 characters");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .Must(x => x.Date >= DateTime.Today)
                .WithMessage("Start date must be today or in the future");

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("End date must be equal to or after start date");

            RuleFor(x => x.VacationType)
                .NotEmpty()
                .Must(x => new[] { "S", "U", "A", "O", "B" }.Contains(x))
                .WithMessage("Invalid vacation type");

            RuleFor(x => x.TotalVacationDays)
                .GreaterThan(0)
                .WithMessage("Total vacation days must be greater than 0");
        }
    }
}
