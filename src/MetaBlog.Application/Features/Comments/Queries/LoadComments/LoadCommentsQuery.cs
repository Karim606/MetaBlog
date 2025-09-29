using MediatR;
using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Comments.Dtos;
using MetaBlog.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Comments.Queries.LoadComments
{
    public record LoadCommentsQuery(Guid postId, Guid? parentCommentId,DateTime? lastCommentTime,int pageSize): IRequest<Result<PaginatedListCursorBased<CommentDto, DateTime?>>>;



}
