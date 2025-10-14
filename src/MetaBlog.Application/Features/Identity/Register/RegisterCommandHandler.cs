using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.RepositoriesInterfaces;
using MetaBlog.Domain.Users;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Identity.Register
{
    public class RegisterCommandHandler(IIdentityService identityService,IDomainUserRepository domainUserRepository,ILogger<RegisterCommandHandler>logger) : IRequestHandler<RegisterCommand, Result<Created>>
    {
        public async Task<Result<Created>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var result = await identityService.RegisterUserAsync(request.Email, request.Password);
            if (result.IsSuccess)
            {
                logger.LogInformation("User {Email} registered successfully.", request.Email);
                var user = User.Create(result.Value,request.Dob,request.firstName,request.lastName);
                await domainUserRepository.AddUserAsync(user);

                return Result.Created;
            }
            else { 
                logger.LogWarning("Failed registration attempt for user {Email}. Reason: {Reason}", request.Email, result.TopError.ToLogObject());
                return result.Errors!; }
        }
    }
}
