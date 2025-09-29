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
using MetaBlog.Domain.Comments;
namespace MetaBlog.Application.Features.Comments.Commands.AddComment
{
    public class AddCommentCommandHandler(ICommentRepository commentRepository,ICurrentUserService currentUserService,ILogger<AddCommentCommandHandler> logger)
        : IRequestHandler<AddCommentCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var parseResult = Guid.TryParse(currentUserService.GetId(),out Guid userId);
            if (!parseResult) { 
                logger.LogWarning("User not authenticated");
                return Error.Unauthorized();
            }
            var comment =  Comment.Create(new Guid(),request.postId, userId, request.content,request.parentCommentId);
            await commentRepository.AddCommentAsync(comment, cancellationToken);
            return comment.Id;
        }
    }
}
