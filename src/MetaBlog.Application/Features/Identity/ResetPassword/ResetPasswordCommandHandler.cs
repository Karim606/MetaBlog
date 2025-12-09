using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.Users;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Identity.ResetPassword
{
    public class ResetPasswordCommandHandler(ILogger<ResetPasswordCommandHandler>logger,IIdentityService identityService)
        : IRequestHandler<ResetPasswordCommand, Result<Success>>
    {
        public async Task<Result<Success>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await identityService.ResetPasswordAsync(request.model.email, request.model.token, request.model.newPassword);

            if (result.IsSuccess)
            {
                logger.LogInformation("User with email {Email} reset their password successfully.", request.model.email);
            }
            else
            {
                logger.LogWarning("User with email {Email} failed to reset their password. Reason: {Reason}",
                    request.model.email, result.TopError);
            }
            return result;
        }
    }
}
