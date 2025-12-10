using FluentValidation;
using MetaBlog.Application.Features.Identity.Dto.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Identity.Register
{
    public class RegisterCommandValidatior:AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidatior() {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(10);
            RuleFor(x => x.Password).Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.");
            RuleFor(x => x.Password).Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.");
            RuleFor(x => x.Password).Matches("[0-9]").WithMessage("Password must contain at least one number.");
            RuleFor(x => x.firstName).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.lastName).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.Dob).LessThan(DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-18))).WithMessage("You must be at least 18 years old to register.");
            RuleFor(x => x.Bio).MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Bio));

            RuleFor(x => x.ProfileImage)
            .Must(f => f == null || f.Length <= 5 * 1024 * 1024).WithMessage("File must be <= 5 MB") 
            .Must(f => f == null || new[] { "image/jpeg", "image/png", "image/webp" }.Contains(f.ContentType))
            .WithMessage("Unsupported file type");
        }
    }
}
