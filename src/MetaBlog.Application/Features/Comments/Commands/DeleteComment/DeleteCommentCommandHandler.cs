using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.RepositoriesInterfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Comments.Commands.DeleteComment
{
    public class DeleteCommentCommandHandler(ICommentRepository commentRepository,ICurrentUserService userService,ILogger<DeleteCommentCommandHandler>logger)
        : IRequestHandler<DeleteCommentCommand, Result<Deleted>>
    {
        public async Task<Result<Deleted>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(userService.GetId());
            var comment = await commentRepository.GetCommentByIdAsync(request.id, cancellationToken);
            if (comment == null)
            {
                return Error.NotFound("Comment not found");
            }

            if (userId == Guid.Empty || userId != comment.userId)
            {
                logger.LogWarning("User not authenticated");
                return Error.Unauthorized();
            }
            await commentRepository.DeleteCommentAsync(comment, cancellationToken);
            return Result.Deleted;
        }
    }
}
