using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Comments;
using MetaBlog.Application.Features.Comments.Dtos;
using MetaBlog.Domain.Comments;
using MetaBlog.Domain.Posts;
using MetaBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MetaBlog.Infrastructure.QueryServices.CommentQueryService
{
    public class CommentQueryService : ICommentQueryService
    {
        private readonly AppDbContext _context;

        public CommentQueryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedListCursorBased<CommentDto, DateTime?>> GetRepliesAsync(Guid parentCommentId, DateTime? lastCommentTime, int pageSize, CancellationToken cancellationToken)
        {
            var query = _context.Comments.Where(c => c.parentCommentId == parentCommentId);
            return await GetComments(query, lastCommentTime, pageSize, _context);
        }

        public async Task<PaginatedListCursorBased<CommentDto, DateTime?>> GetCommentsAsync(Guid postId, DateTime? lastCommentTime, int pageSize, CancellationToken cancellationToken)
        {
            var query = _context.Comments.Where(c => c.postId == postId && c.parentCommentId == null);
            return await GetComments(query, lastCommentTime, pageSize, _context);
        }

        private static async Task<PaginatedListCursorBased<CommentDto, DateTime?>> GetComments(IQueryable<Comment> query, DateTime? lastCommentTime, int pageSize, AppDbContext context)
        {
            query = query.OrderByDescending(c => c.createdAt);

            if (lastCommentTime != null)
            {
                query = query.Where(c => c.createdAt <= lastCommentTime);
            }
            query = query.Take(pageSize + 1);
            
              var items = await ( from c in query
                            join r in context.Comments on c.Id equals r.parentCommentId into replies
                            join u in context.IdentityAppUsers on c.userId equals u.Id
                            select new CommentDto
                            {
                                Id = c.Id,
                                userId = c.userId,
                                Content = c.content,
                                parentCommentId = c.parentCommentId,
                                postId = c.postId,
                                createdAt = c.createdAt.LocalDateTime,
                                repliesCount = replies.Count(),
                                likesCount = c.likesCount,
                                userName = u.FirstName + " " + u.LastName
                            } ).AsNoTracking().ToListAsync();


            var hasMore = items.Count > pageSize;
            DateTime? cursor = hasMore ? items.Last().createdAt : null;

            if (hasMore)
                items.RemoveAt(pageSize);

            return new PaginatedListCursorBased<CommentDto, DateTime?>
            {
                PageSize = pageSize,
                NextCursor = cursor,
                Items = items
            };
        }

        public async Task<CommentDto> GetCommentByIdAsync(Guid id, CancellationToken ct)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if(comment == null)
                return null;

            return new CommentDto
            {
                createdAt = comment.createdAt.LocalDateTime,
                Content = comment.content,
                likesCount = comment.likesCount,
                parentCommentId = comment.parentCommentId,
                postId = comment.postId,
                Id = id
            };
        }
    }
}
