using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Favorites;
using MetaBlog.Application.Features.Favorites.Dtos.Response;
using MetaBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Infrastructure.QueryServices.FavoriteQueryService
{
    public class FavoriteQueryService(AppDbContext context) : IFavoriteQueryService
    {
        public async Task<PaginatedList<FavoriteDto>> GetFavorites(int page, int pageSize, Guid userId, CancellationToken ct)
        {
           var items = await context.Favorites.Where(f => f.userId == userId).Skip((page - 1) * pageSize).Take(pageSize)
                        .Select(f => new FavoriteDto
                        {
                            PostId = f.postId,
                            Id = f.Id,
                            Title = f.post.Title
                        }).ToListAsync(ct);
            var totalCount = await context.Favorites.CountAsync(ct);
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            return new PaginatedList<FavoriteDto>
            {
                Items = items,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalCount = totalCount,
            };
        }
    }
}
