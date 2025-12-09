using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Likes.Dtos;
using MetaBlog.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Likes.Queries.GetLikes
{
    public class GetLikesQueryHandler(ILikeQueryService likeQueryService,ICurrentUserService currentUserService) : IRequestHandler<GetLikesQuery, Result<PaginatedList<LikeDto>>>
    {
        public async Task<Result<PaginatedList<LikeDto>>> Handle(GetLikesQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.GetId();

          var list = await likeQueryService.GetLikesAsync(request.targetId, request.type, Guid.Parse(userId), request.offset, request.pageSize);

            if (list.Items == null)
                return Error.NotFound(description: "target not found");

            return list;
        }
    }
}
