using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.RepositoriesInterfaces;
using Microsoft.Extensions.Logging;
using MetaBlog.Domain.Follows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Follow.Commands.FollowUser
{
    public class FollowCommandHandler(ILogger<FollowCommandHandler>logger,IFollowRepository followRepository,ICurrentUserService currentUserService,
        IDomainUserRepository userRepository) : IRequestHandler<FollowCommand, Result<Success>>
    {
        public async Task<Result<Success>> Handle(FollowCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty) { return Error.Validation(description: "followed id cant be empty"); }
           
            var user = userRepository.Equals(await userRepository.GetByIdAsync(request.Id));
            if (user == null)
            {
                logger.LogWarning("User with Id {FollowedId} does not exist", request.Id);
                return Error.NotFound(description:"The user you are trying to follow does not exist.");
            }

            var followerId = Guid.Parse(currentUserService.GetId());

            var followRequest = await followRepository.GetFollowAsync(followerId, request.Id);
            if(followRequest != null)
            {
                logger.LogWarning("User {FollowerId} is already following User {FollowedId}", followerId, request.Id);
                return Error.Conflict(description:"You are already following this user.");
            }
            
            var result = Domain.Follows.Follow.Create(followerId, request.Id);
            if (result.IsError) { 
                logger.LogInformation("Failed to create follow relationship: {ErrorMessage}", result.TopError.Description);
                return Error.Conflict(description:"Failed to create follow relationship");
            }

            await followRepository.FollowUserAsync(Domain.Follows.Follow.Create(followerId, request.Id).Value);
            
            return Result.Success;
        }
    }
}
