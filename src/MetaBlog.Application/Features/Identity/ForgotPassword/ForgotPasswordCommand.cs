using MediatR;
using MetaBlog.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Identity.ForgotPassword
{
    public record ForgotPasswordCommand(string Email):IRequest<Result<Success>>;
    
}
