using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Identity.ForgotPassword
{
    public class ForgotPasswordCommandValidator:AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator() { 
            RuleFor(c => c.Email).EmailAddress().NotNull().NotEmpty();
        }
    }
}
