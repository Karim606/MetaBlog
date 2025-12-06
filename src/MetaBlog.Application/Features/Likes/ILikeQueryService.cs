using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Likes.Dtos;
using MetaBlog.Domain.Likes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Likes
{
    public interface ILikeQueryService
    {
        Task<PaginatedList<LikeDto>> GetLikesAsync(Guid targetId,LikeTargetType type, Guid currentUserId, int offset, int pageSize);
    }
}
