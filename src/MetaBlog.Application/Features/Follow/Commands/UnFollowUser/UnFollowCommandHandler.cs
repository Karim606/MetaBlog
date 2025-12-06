using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Application.Features.Follow.Commands.FollowUser;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.RepositoriesInterfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Follow.Commands.UnFollowUser
{
    public class UnFollowCommandHandler(ILogger<UnFollowCommandHandler> logger, IFollowRepository followRepository, ICurrentUserService currentUserService,
        IDomainUserRepository userRepository)
        :IRequestHandler<UnFollowCommand, Result<Success>>
    {
        public async Task<Result<Success>> Handle(UnFollowCommand request, CancellationToken cancellationToken)
        {
            if (request.followedId == Guid.Empty) { return Error.Validation(description: "followed id cant be empty"); }
            var user = await userRepository.GetByIdAsync(request.followedId);

            if (user == null)
            {
                logger.LogWarning("User with Id {FollowedId} does not exist", request.followedId);
                return Error.NotFound(description: "The user you are trying to follow does not exist.");
            }

            var followerId = Guid.Parse(currentUserService.GetId());

            if (followerId == request.followedId) { return Error.Conflict(description: "user cant unfollow himself"); }

            var follow = await followRepository.GetFollowAsync(followerId, request.followedId);
            if (follow == null)
            {
                logger.LogWarning("User {FollowerId} is already not following User {FollowedId}", followerId, request.followedId);
                return Error.Conflict(description: "You are already not following this user.");
            }

            await followRepository.UnfollowUserAsync(follow);

            return Result.Success;
        }
    }
    
}
