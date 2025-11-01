using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Domain.Common.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Identity.ForgotPassword
{
    public class ForgotPasswordCommandHandler(ILogger<ForgotPasswordCommand>logger,IIdentityService identityService) : IRequestHandler<ForgotPasswordCommand,Result<Success>>
    {
        public async Task<Result<Success>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await identityService.RequestResetPasswordAsync(request.Email);
            if(result.IsSuccess)
            return result;
            else
            {
                logger.LogWarning("request to reset password for email: {email} has been failed error:{error}", request.Email, result.TopError);
                return Error.Conflict(description: "something went wrong");
            }
        }

    }
}
