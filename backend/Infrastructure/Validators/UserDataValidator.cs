using Domain.Entity;
using FluentValidation;

namespace Domain.Validators
{
    public class UserDtoValidator : AbstractValidator<UserData>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.Phone)
                .Matches(@"^\+\d{10,15}$")
                .WithMessage("Phone number must start with '+' followed by 10 to 15 digits.");

            RuleFor(x => x.Salary)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Salary cannot be negative.");

            RuleFor(x => x.DateOfBirth)
                .NotNull()
                .WithMessage("Date of birth is required.")
                .Must(date => date <= DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Date of birth cannot be in the future.");
        }
    }
}
