using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Application.Features.Identity.Dto.Responses;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.RefreshTokens;
using MetaBlog.Domain.Users;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MetaBlog.Domain.RepositoriesInterfaces;
namespace MetaBlog.Application.Features.Identity.Login
{
    public class LoginCommandHandler(IDomainUserRepository domainUserRepository,IIdentityService identityService,IJwtService jwtService,
                  ICurrentRequestContext currentRequestContext,IRefreshTokenRepository refreshTokenRepository,ILogger<LoginCommandHandler>logger) : IRequestHandler<LoginCommand, Result<Token>>

    {
        public async Task<Result<Token>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await identityService.LoginAsync(request.Email, request.Password);
            if (result.IsSuccess)
            {

                logger.LogInformation("User {Email} logged in successfully.", request.Email);
                var user = await domainUserRepository.GetByIdAsync(result.Value.Item1);
                var accessToken = jwtService.GenerateToken($"{user.firstName} {user.lastName}", request.Email!, user.Id, result.Value.Item2);
                var (unHashedRefreshToken, expiresAt) = jwtService.GenerateRefreshToken();
                var refreshToken = MetaBlog.Domain.RefreshTokens.RefreshToken.Create(new Guid(), user.Id, jwtService.HashToken(unHashedRefreshToken),
                    expiresAt,currentRequestContext.IpAddress,currentRequestContext.DeviceInfo);
                
                await refreshTokenRepository.AddTokenAsync(refreshToken);
                
                var token = new Token(accessToken,unHashedRefreshToken,expiresAt);
                return token;

            }
            else
            {
                logger.LogWarning("Failed login attempt for user {Email}. Reason: {Reason}", request.Email, result.TopError.ToLogObject());
                return result.Errors!;
            }
        }
    }
}
