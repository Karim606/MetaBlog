using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Follow;
using MetaBlog.Application.Features.Follow.Dtos.response;
using MetaBlog.Application.Features.Posts.Dtos.Response;
using MetaBlog.Domain.Follows;
using MetaBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Infrastructure.QueryServices.FollowQueryService
{
    public class FollowQueryService(AppDbContext context) : IFollowQueryService
    {
        public async Task<PaginatedList<Followers_FollowedDto>> GetFollowsAsync(Guid userId, FollowQueryType queryType, int pageNumber, int pageSize, string? searchTerm,
        DateTime? createdAfter, bool? sortDescending, CancellationToken ct)
        {
            IQueryable<Follow> query = queryType == FollowQueryType.Followers
                ? context.Follows.Where(f => f.FollowedId == userId)
                : context.Follows.Where(f => f.FollowerId == userId);

            query = query
                .ApplySorting(sortDescending)
                .ApplySearch(queryType == FollowQueryType.Followers?FollowSearchOptions.FollowerName:FollowSearchOptions.FollowedName, searchTerm);

            var list = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new Followers_FollowedDto
                {
                    id = queryType == FollowQueryType.Followers ? f.FollowerId : f.FollowedId,
                    name = queryType == FollowQueryType.Followers
                        ? f.Follower.firstName + " " + f.Follower.lastName
                        : f.Followed.firstName + " " + f.Followed.lastName,
                    imageUrl = queryType == FollowQueryType.Followers
                        ? f.Follower.imageUrl
                        : f.Followed.imageUrl
                })
                .AsNoTracking()
                .ToListAsync(ct);

            var totalCount = await query.CountAsync(ct);
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PaginatedList<Followers_FollowedDto>
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = list
            };
        }
    }
 }
