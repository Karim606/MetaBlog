using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Application.Features.Posts.Commands.EditPost;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.RepositoriesInterfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Posts.Commands.DeletePost
{
    public class DeletePostCommandHandler(IPostRepository postRepository, ICurrentUserService currentUserService, ILogger<DeletePostCommandHandler> logger) 
        : IRequestHandler<DeletePostCommand, Result<Deleted>>
    {
        public async Task<Result<Deleted>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await postRepository.GetPostByIDAsync(request.Id, cancellationToken);

            var userId = Guid.Parse(currentUserService.GetId());

            if (post == null)
            {
                logger.LogInformation("Post with id {PostId} not found", request.Id);
                return Error.NotFound("Post not found");
            }

            if (post.authorId != userId)
            {
                logger.LogWarning("User {UserId} attempted to Delete post {PostId} but is not the author", userId, request.Id);
                return Error.NotFound("Post not found");
            }

            await postRepository.DeletePostAsync(post, cancellationToken);
            await postRepository.SaveChangesAsync(cancellationToken);
            return Result.Deleted;
        }
    }
}
