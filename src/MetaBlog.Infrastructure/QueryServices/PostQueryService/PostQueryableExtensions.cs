using MetaBlog.Domain.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Infrastructure.QueryServices.PostQueryService
{
    public static class PostQueryableExtensions
    {
        public static IQueryable<Post>ApplySearch(this IQueryable<Post>query,string searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => p.Title.Contains(searchTerm) || p.Content.Contains(searchTerm));
            }
            return query;
        }
        public static IQueryable<Post>ApplyFilterWithAuthorId(this IQueryable<Post>query,Guid? authorId)
        {
            if (authorId.HasValue)
            {
                query = query.Where(p => p.authorId == authorId.Value);
            }
            return query;
        }
        public static IQueryable<Post>ApplyFilterWithCreatedAfter(this IQueryable<Post>query,DateTime? createdAfter)
        {
            if (createdAfter.HasValue)
            {
                query = query.Where(p => p.createdAt >= createdAfter.Value);
            }
            return query;
        }
        public static IQueryable<Post>ApplySorting(this IQueryable<Post>query,string? sortBy,bool? sortDescending)
        {
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "title" => sortDescending == true ? query.OrderByDescending(p => p.Title) : query.OrderBy(p => p.Title),
                    "createdat" => sortDescending == true ? query.OrderByDescending(p => p.createdAt) : query.OrderBy(p => p.createdAt),
                    _ => query.OrderByDescending(p => p.createdAt)
                };
            }

             return query;
        }
    }
}
