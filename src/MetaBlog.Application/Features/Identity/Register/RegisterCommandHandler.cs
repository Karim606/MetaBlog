using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Application.Features.Identity.Dtos.Responses;
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
    public class RegisterCommandHandler(IIdentityService identityService,IJwtService jwtService,ICurrentRequestContext currentRequestContext,
        IDomainUserRepository domainUserRepository,ILogger<RegisterCommandHandler>logger,IRefreshTokenRepository refreshTokenRepository,
        IImageService imageService) 
        : IRequestHandler<RegisterCommand, Result<RefreshTokenResponseDto>>
    {
        public async Task<Result<RefreshTokenResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var result = await identityService.RegisterUserAsync(request.Email, request.Password);
            if (result.IsSuccess)
            {
                logger.LogInformation("User {Email} registered successfully.", request.Email);
                var user = User.Create(result.Value,request.Dob,request.firstName,request.lastName);

                string? imageUrl = null;

                if (request.ProfileImage!=null&&request.ProfileImage.Length>0) {
                    var resultOfUploading = await imageService.UploadAsync(request.ProfileImage);

                    if (resultOfUploading.IsSuccess)
                    {
                        imageUrl= resultOfUploading.Value;
                    }
                }
                user.Update(bio:request.Bio,imageUrl:imageUrl,null,null);
                await domainUserRepository.AddUserAsync(user);

                var accessToken = jwtService.GenerateToken($"{user.firstName} {user.lastName}", request.Email!, user.Id, new List<string> {"User"});

                var (unHashedRefreshToken, expiresAt) = jwtService.GenerateRefreshToken();

                var refreshToken = MetaBlog.Domain.RefreshTokens.RefreshToken.Create(new Guid(), user.Id, jwtService.HashToken(unHashedRefreshToken),
                    expiresAt, currentRequestContext.IpAddress, currentRequestContext.DeviceInfo);

                await refreshTokenRepository.AddTokenAsync(refreshToken);

                var token = new RefreshTokenResponseDto(accessToken, unHashedRefreshToken, expiresAt);
                
                return token;

            }

            else { 
                logger.LogWarning("Failed registration attempt for user {Email}. Reason: {Reason}", request.Email, result.TopError.ToLogObject());
                if(result.TopError.Type==ErrorKind.Conflict)
                return Error.Conflict(description: "We can’t complete registration with this email. If you already have an account, please sign in or reset your password.");

                return Error.Failure();
            }
        }
    }
}
