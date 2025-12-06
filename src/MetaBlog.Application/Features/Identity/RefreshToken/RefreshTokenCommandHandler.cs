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
           var oldToken = await refreshTokenRepository.GetByHashTokenAsync(hashedToken);
            if (oldToken == null||oldToken.expiresAt<DateTime.UtcNow)
            {
              
                logger.LogWarning("refresh Token is invalid TokenID:{tokenId} is invalid",oldToken?.Id);
                return Error.Unauthorized("");
                
            }
            var (newRefreshToken,expiresAt) = jwtService.GenerateRefreshToken();
            var newHashedToken = jwtService.HashToken(newRefreshToken);

            var userId = oldToken.userId;

            var newToken = MetaBlog.Domain.RefreshTokens.RefreshToken.Create(Guid.NewGuid(),userId
                                        ,newHashedToken,expiresAt,currentRequestContext.IpAddress,currentRequestContext.DeviceInfo);
            
            await refreshTokenRepository.AddTokenAsync(newToken);
            oldToken.Revoke(newToken.Id, RevokeReasons.Rotated);

            await refreshTokenRepository.SaveChangesAsync();
           

            var user = await domainUserRepository.GetByIdAsync(userId);

            var resultOfRoles = await identityService.GetUserRolesAsync(userId);
            var resultOfEmail = await identityService.GetUserEmailAsync(userId);

            var accessToken = jwtService.GenerateToken(user.firstName + " " + user.lastName,resultOfEmail.Value,userId,resultOfRoles.Value);

           return new RefreshTokenResponseDto
            {
                accessToken = accessToken,
                expiresAt = expiresAt,
                refreshToken = newRefreshToken
            };



        }
    }
}
