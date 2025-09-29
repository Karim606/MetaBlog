using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.Posts;
using MetaBlog.Domain.RepositoriesInterfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Posts.Commands.EditPost
{
    public class EditPostCommandHandler(IPostRepository postRepository,ICurrentUserService currentUserService,ILogger<EditPostCommandHandler>logger)
        : IRequestHandler<EditPostCommand, Result<Updated>>
    {
        public async Task<Result<Updated>> Handle(EditPostCommand request, CancellationToken cancellationToken)
        {
            var post = await postRepository.GetPostByIDAsync(request.PostId, cancellationToken);
           
            var userId = Guid.Parse(currentUserService.GetId());

            if (post == null)
            {
                logger.LogInformation("Post with id {PostId} not found", request.PostId);
                return Error.NotFound("Post not found");
            }

            if (post.authorId != userId)
            {
                logger.LogWarning("User {UserId} attempted to access post {PostId} but is not the author", userId, request.PostId);
                return Error.NotFound("Post not found");
            }

            if (post.Title == request.Title&&post.Content==request.Content)
            {
               return Error.Conflict("No changes detected");
            }
            post.Update(request.Title, request.Content);
            await postRepository.SaveChangesAsync(cancellationToken);
            return Result.Updated;
        }
    }
}
