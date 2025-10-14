using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Application.Features.Identity.Dtos.Responses;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.RefreshTokens;
using MetaBlog.Domain.RepositoriesInterfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Identity.RefreshToken
{
    public class RefreshTokenCommandHandler(IJwtService jwtService,IRefreshTokenRepository refreshTokenRepository,
        ICurrentUserService currentUserService,ICurrentRequestContext currentRequestContext,ILogger<RefreshTokenCommandHandler> logger,
        IDomainUserRepository domainUserRepository,IIdentityService identityService
        )
        : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponseDto>>
    {
        public async Task<Result<RefreshTokenResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
           var hashedToken =  jwtService.HashToken(request.unHashedRefreshToken);
           var storedToken = await refreshTokenRepository.GetByHashTokenAsync(hashedToken);
            if (storedToken == null||storedToken.expiresAt<DateTime.UtcNow)
            {
              
                logger.LogWarning("refresh Token for user {UserId} is invalid TokenID:{tokenId} is invalid",currentUserService.GetId(),storedToken?.Id);
                return Error.Unauthorized("");
                
            }
            var (newRefreshToken,expiresAt) = jwtService.GenerateRefreshToken();
            var hashedRefreshToken = jwtService.HashToken(newRefreshToken);


            var storedRefreshToken = MetaBlog.Domain.RefreshTokens.RefreshToken.Create(Guid.NewGuid(),storedToken.userId
                                        ,hashedRefreshToken,expiresAt,currentRequestContext.IpAddress,currentRequestContext.DeviceInfo);

            storedToken.Revoke(storedRefreshToken.Id, RevokeReasons.Rotated);

            await refreshTokenRepository.SaveChangesAsync();
            await refreshTokenRepository.AddTokenAsync(storedRefreshToken);

            var user = await domainUserRepository.GetByIdAsync(storedToken.userId);

            var resultOfRoles = await identityService.GetUserRolesAsync(storedToken.Id);
            var resultOfEmail = await identityService.GetUserEmailAsync(storedToken.Id);

            var accessToken = jwtService.GenerateToken(user.firstName + " " + user.lastName,resultOfEmail.Value,storedToken.Id,resultOfRoles.Value);

           return new RefreshTokenResponseDto
            {
                accessToken = accessToken,
                expiresAt = expiresAt,
                refreshToken = newRefreshToken
            };



        }
    }
}
