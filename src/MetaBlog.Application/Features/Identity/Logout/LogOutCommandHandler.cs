using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.RefreshTokens;
using MetaBlog.Domain.RepositoriesInterfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Identity.Logout
{
    public class LogOutCommandHandler(IRefreshTokenRepository refreshTokenRepository,IJwtService jwtService,ILogger<LogOutCommandHandler> logger)
        : IRequestHandler<LogOutCommand, Result<Success>>
    {
        public async Task<Result<Success>> Handle(LogOutCommand request, CancellationToken cancellationToken)
        {
            if (request.refreshToken ==null ) {
                logger.LogWarning("No refresh token provided during logout.");
                return  Error.Unauthorized();
            }
            var hashedToken = jwtService.HashToken(request.refreshToken);
            var storedToken = await refreshTokenRepository.GetByHashTokenAsync(hashedToken);

            if (storedToken == null) {
                logger.LogWarning("Refresh token not found");
                return Error.Unauthorized(); }

            if (storedToken.revokedAt.HasValue)
            {
                logger.LogInformation("Refresh token already revoked.");
                return Error.Unauthorized();
            }
            storedToken.Revoke(Guid.Empty, RevokeReasons.LoggedOut);
            await refreshTokenRepository.SaveChangesAsync();

            logger.LogInformation("User {UserId} successfully logged out.", storedToken.userId);

            return Result.Success;
        }
    }
}
