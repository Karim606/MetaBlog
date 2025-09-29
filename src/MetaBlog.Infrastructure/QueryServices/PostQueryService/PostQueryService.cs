using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Posts;
using MetaBlog.Application.Features.Posts.Dtos.Response;
using MetaBlog.Domain.Common;
using MetaBlog.Domain.Posts;
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
             var post =await context.Posts.Include(p=> p.Comments).Include(p=>p.User).FirstOrDefaultAsync(p => p.Id == Id,ct);
            IdentityAppUser user;
            if (post != null)
         user = await context.IdentityAppUsers.FirstOrDefaultAsync(u => u.Id == post.User.Id,ct);
            
            else return null;

                return new PostDto
                {
                    Id=post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    CommentsCount = post.Comments.Count(),
                    LikesCount = post.likesCount,
                    Slug = post.Slug,
                    UserName = user.FirstName+user.LastName
                };
            
        }

        public async Task<PaginatedList<PostDto>> GetPostsAsync(int pageNumber, int pageSize, string? searchTerm, Guid? authorId, DateTime? createdAfter, string? sortBy, bool? sortDescending, CancellationToken ct)
        {
            var query = context.Posts.AsQueryable();

            query.ApplySearch(searchTerm)
                .ApplyFilterWithAuthorId(authorId)
                .ApplyFilterWithCreatedAfter(createdAfter)
                .ApplySorting(sortBy,sortDescending);
           

            var items = await context.Posts.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Join(context.IdentityAppUsers,p => p.User.Id, u => u.Id, (posts, users)=>new { posts,users }).Select(x => new PostDto{
                        Content = x.posts.Content,
                        Title = x.posts.Title,
                        Slug = x.posts.Slug,
                        CommentsCount = x.posts.Comments.Count(),
                        LikesCount = x.posts.likesCount,

                    UserName = x.users.FirstName+" "+x.users.LastName
                        }).AsNoTracking().ToListAsync(ct);

            var totalCount = await context.Posts.CountAsync(ct);
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PaginatedList<PostDto> {
            TotalCount = totalCount,
            TotalPages = totalPages,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Items = items
            };
        }

        
    }
}

