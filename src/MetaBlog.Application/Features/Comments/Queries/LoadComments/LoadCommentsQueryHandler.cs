using MediatR;
using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Comments.Dtos;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.Common.Results.Interface;
using MetaBlog.Domain.RepositoriesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Comments.Queries.LoadComments
{
    public class LoadCommentsQueryHandler(ICommentQueryService commentQueryService) : IRequestHandler<LoadCommentsQuery, Result< PaginatedListCursorBased<CommentDto, DateTime?>> >
    {

        public async Task<Result<PaginatedListCursorBased<CommentDto, DateTime?>>> Handle(LoadCommentsQuery request, CancellationToken cancellationToken)
        {
            var result=new PaginatedListCursorBased<CommentDto, DateTime?>();
            if (request.parentCommentId == null)
            {
                 result = await commentQueryService.GetCommentsAsync(request.postId, request.lastCommentTime, request.pageSize, cancellationToken);
            }
            else
            {
                result = await commentQueryService.GetRepliesAsync(request.parentCommentId.Value, request.lastCommentTime, request.pageSize, cancellationToken);
            }
            if (!result.Items!.Any()) { return Error.NotFound("Post Not found"); }

            return result;
        }
    }
}
