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
            return result;
        }

    }
}
