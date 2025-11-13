using MediatR;
using MetaBlog.Application.Features.Users.Dtos;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.RepositoriesInterfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Users.Queries.GetUserProfile
{
    public class GetUserProfileQueryHandler(IDomainUserRepository domainUserRepository,ILogger<GetUserProfileQueryHandler>logger)
        : IRequestHandler<GetUserProfileQuery, Result<UserProfileDto>>
    {
        public async Task<Result<UserProfileDto>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetUserProfileQuery for UserId: {UserId}", request.UserId);
            var user = await domainUserRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                logger.LogWarning("User with Id: {UserId} not found.", request.UserId);
                return Error.NotFound(description:$"User with Id {request.UserId} not found.");
            }

            var userProfileDto = new UserProfileDto
            {
                id = user.Id,
                bio = user.Bio,
                imageUrl = user.imageUrl,
                userName = $"{user.firstName} {user.lastName}"
            };
            logger.LogInformation("Successfully retrieved profile for UserId: {UserId}", request.UserId);
            return userProfileDto;
        }
    }
}
