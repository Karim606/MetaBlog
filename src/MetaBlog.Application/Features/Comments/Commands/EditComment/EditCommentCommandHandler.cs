using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Domain.Comments;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.RepositoriesInterfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Comments.Commands.EditComment
{
    public class EditCommentCommandHandler(ICommentRepository commentRepository,ICurrentUserService currentUserService,ILogger<EditCommentCommandHandler>logger)
        : IRequestHandler<EditCommentCommand, Result<Updated>>
    {
        public async Task<Result<Updated>> Handle(EditCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(currentUserService.GetId());
            var comment = await commentRepository.GetCommentByIdAsync(request.id, cancellationToken);
            if (comment == null) {
            return Error.NotFound("Comment not found");
            }

            if (userId == Guid.Empty||userId!=comment.userId)
            {
                logger.LogWarning("User not authenticated");
                return Error.Unauthorized();
            }
            comment.UpdateContent(request.content);
            await commentRepository.SaveChangesAsync(cancellationToken);
            return Result.Updated;

        }
    }
}
