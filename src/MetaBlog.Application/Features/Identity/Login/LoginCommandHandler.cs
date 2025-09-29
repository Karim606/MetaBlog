using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Application.Features.Identity.Dto.Responses;
using MetaBlog.Domain.Common.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Identity.Login
{
    public class LoginCommandHandler(IIdentityService identityService,ILogger<LoginCommandHandler>logger) : IRequestHandler<LoginCommand, Result<Token>>
    {
        public async Task<Result<Token>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await identityService.LoginAsync(request.Email, request.Password);
            if (result.IsSuccess)
            {
                logger.LogInformation("User {Email} logged in successfully.", request.Email);
                return new Token(result.Value!);
            }
            else
            {
                logger.LogWarning("Failed login attempt for user {Email}. Reason: {Reason}", request.Email, result.TopError.ToLogObject());
                return result.Errors!;
            }
        }
    }
}
