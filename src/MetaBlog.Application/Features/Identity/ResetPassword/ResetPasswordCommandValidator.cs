using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Identity.ResetPassword
{
    public class ResetPasswordCommandValidator:AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(c => c.model.email)
            .NotEmpty().WithMessage("Email cannot be empty or whitespace.")
            .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(c => c.model.newPassword)
                .NotEmpty().WithMessage("New password cannot be empty or whitespace.")
                .MinimumLength(10).WithMessage("Password must be at least 10 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.");


            RuleFor(c => c.model.token)
                .NotEmpty().WithMessage("Token cannot be empty or whitespace.");

        }
    }
}
