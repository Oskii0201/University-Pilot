using FluentValidation;
using UniversityPilot.BLL.Areas.Identity.DTO;
using UniversityPilot.DAL;

namespace UniversityPilot.BLL.Areas.Identity.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(UniversityPilotContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Email).Custom((value, context) =>
            {
                var emailInUse = dbContext.Users.Any(u => u.Email == value);
                if (emailInUse)
                {
                    context.AddFailure("email", "That email is taken");
                }
            });

            RuleFor(x => x.Password)
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(e => e.Password).WithMessage("Passwords do not match.");
        }
    }
}