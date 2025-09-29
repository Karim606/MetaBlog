using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Comments.Dtos;
using MetaBlog.Domain.Comments;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Comments
{
    public interface  ICommentQueryService
    {
        public Task<PaginatedListCursorBased< CommentDto, DateTime?> > GetCommentsAsync(Guid postId, DateTime? lastCommentTime, int pageSize, CancellationToken cancellationToken);
        public Task<PaginatedListCursorBased< CommentDto, DateTime?>  > GetRepliesAsync(Guid parentCommentId, DateTime? lastCommentTime, int pageSize, CancellationToken cancellationToken);
        public Task<CommentDto> GetCommentByIdAsync(Guid id,CancellationToken ct);
    }
}
