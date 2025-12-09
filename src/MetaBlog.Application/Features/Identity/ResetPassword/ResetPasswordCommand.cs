using MediatR;
using MetaBlog.Application.Features.Identity.Dtos.Requests;
using MetaBlog.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Identity.ResetPassword
{
    public record ResetPasswordCommand(ResetPasswordDto model):IRequest<Result<Success>>;
    
}
