using MediatR;
using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Likes.Dtos;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.Likes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Likes.Queries.GetLikes
{
    public record GetLikesQuery(Guid targetId,LikeTargetType type,int pageSize,int offset):IRequest<Result<PaginatedList<LikeDto>>>;
    
}
