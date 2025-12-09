using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Likes;
using MetaBlog.Application.Features.Likes.Dtos;
using MetaBlog.Domain.Common.Interfaces;
using MetaBlog.Domain.Likes;
using MetaBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Infrastructure.QueryServices.LikesQueryServer
{
     public class LikesQueryService(AppDbContext context): ILikeQueryService
    {
        private readonly AppDbContext _context = context;

        public async Task<PaginatedList<LikeDto>> GetLikesAsync(Guid targetId,LikeTargetType type,Guid currentUserId, int offset, int pageSize)
        {
           var exist = type switch             {
                LikeTargetType.Posts => await _context.Likes.AnyAsync(x => x.TargetType == LikeTargetType.Posts && x.TargetId == targetId),
                LikeTargetType.Comments => await _context.Likes.AnyAsync(x => x.TargetType == LikeTargetType.Comments && x.TargetId == targetId),
                _ => false
                };

            if (!exist)
            {
                return new PaginatedList<LikeDto>
                {
                    Items = null

                };
            }

            var query = _context.Likes.Where(x => x.TargetId == targetId && x.TargetType == type);
            var count = await query.CountAsync();

            var items = query.OrderByDescending(l =>l.likedAt).Skip(offset).Take(pageSize).Select(l => 
                new LikeDto
                {
                    Id = l.Id,
                    TargetId = l.TargetId,
                    TargetType = l.TargetType,
                    UserId = l.userId,
                    LikedAt = l.likedAt.DateTime,
                    imageUrl = l.User.imageUrl,
                    UserName = l.User.firstName +" "+ l.User.lastName,
                    followed = _context.Follows.Any(f => f.Follower.Id == currentUserId && f.Followed.Id == l.userId)
                }   
            );
    
            return new PaginatedList<LikeDto>
            {
                Items = await items.ToListAsync(),
                TotalCount = count,
                PageSize = pageSize,
                Offset= offset,
                
            };
        }
    }
}
