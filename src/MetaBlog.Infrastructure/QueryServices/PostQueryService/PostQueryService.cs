using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Posts;
using MetaBlog.Application.Features.Posts.Dtos.Response;
using MetaBlog.Domain.Common;
using MetaBlog.Domain.Posts;
using MetaBlog.Domain.Users;
using MetaBlog.Infrastructure.Data;
using MetaBlog.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MetaBlog.Infrastructure.QueryServices.PostQueryService
{
    public class PostQueryService(AppDbContext context) : IPostQueryService
    {
        public async Task<PostDto> GetPostByIdAsync(Guid Id,CancellationToken ct)
        {
             var post =await context.Posts.FirstOrDefaultAsync(p => p.Id == Id,ct);
            if (post != null)
                return new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    CommentsCount = post.Comments.Count(),
                    LikesCount = post.likesCount,
                    Slug = post.Slug,
                    UserName = post.User.lastName +" "+ post.User.lastName
                };

            else return null;

               
            
        }

        public async Task<PaginatedList<PostDto>> GetPostsAsync(int pageNumber, int pageSize, string? searchTerm, Guid? authorId, DateTime? createdAfter, string? sortBy, bool? sortDescending,Guid? currentUserId, CancellationToken ct)
        {
            var query = context.Posts.AsQueryable();

           query.ApplySearch(searchTerm)
                .ApplyFilterWithAuthorId(authorId)
                .ApplyFilterWithCreatedAfter(createdAfter)
                .ApplySorting(sortBy,sortDescending);

            var totalCount = await context.Posts.CountAsync(ct);

            var pagedPosts =  query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var items = from p in pagedPosts join
                        l in context.Likes.Where(l => l.userId == currentUserId)
                        on p.Id equals l.TargetId into likesGroup
                        from likes in likesGroup.DefaultIfEmpty()
                        join f in context.Favorites.Where(f => f.userId == currentUserId)
                        on p.Id equals f.postId into favoritesGroup
                        from favorites in favoritesGroup.DefaultIfEmpty()

                        select new PostDto
                        {
                            Id = p.Id,
                            Content = p.Content,
                            Title = p.Title,
                            Slug = p.Slug,
                            CommentsCount = p.Comments.Count(),
                            LikesCount = p.likesCount,
                            IsLikedByCurrentUser = likes != null,
                            IsFavoritedByCurrentUser = favorites != null,
                            UserName = p.User.firstName + " " + p.User.lastName
                        };
            
            var finalItems = await items.AsNoTracking().ToListAsync(ct);

            
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PaginatedList<PostDto> {
            TotalCount = totalCount,
            TotalPages = totalPages,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Items = finalItems
            };
        }

        
    }
}

