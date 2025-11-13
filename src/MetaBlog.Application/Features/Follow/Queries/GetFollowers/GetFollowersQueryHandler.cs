using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Follow.Dtos.response;
using MetaBlog.Application.Features.Follow.Queries.GetFolloweds;
using MetaBlog.Domain.Common.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Follow.Queries.GetFollowers
{
    public class GetFollowersQueryHandler(ILogger<GetFollowedQueryHandler> logger, ICurrentUserService currentUserService, IFollowQueryService queryService)
        : IRequestHandler<GetFollowersQuery, Result<PaginatedList<Followers_FollowedDto>>>
    {
        public async Task<Result<PaginatedList<Followers_FollowedDto>>> Handle(GetFollowersQuery request, CancellationToken cancellationToken)
        {
            var id = Guid.Parse(currentUserService.GetId());

            var result = await queryService.GetFollowsAsync(id, FollowQueryType.Followers, request.pageNumber, request.pageSize, request.searchTerm, request.createdAfter,
                request.sortDescending, cancellationToken);

            return result;
        }
    }
}
