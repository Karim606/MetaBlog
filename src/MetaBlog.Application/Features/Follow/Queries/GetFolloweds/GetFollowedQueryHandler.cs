using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Follow.Dtos.response;
using MetaBlog.Application.Features.Follow.Queries.GetFollowing;
using MetaBlog.Domain.Common.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Follow.Queries.GetFolloweds
{
    public class GetFollowedQueryHandler(ILogger<GetFollowedQueryHandler>logger,ICurrentUserService currentUserService,IFollowQueryService queryService)
        : IRequestHandler<GetFollowedQuery, Result<PaginatedList<Followers_FollowedDto>>>
    {
        public async Task<Result<PaginatedList<Followers_FollowedDto>>> Handle(GetFollowedQuery request, CancellationToken cancellationToken)
        {
           var id = Guid.Parse(currentUserService.GetId());

           var result = await queryService.GetFollowsAsync(id, FollowQueryType.Followed, request.pageNumber, request.pageSize, request.searchTerm, request.createdAfter,
               request.sortDescending,cancellationToken);

            return result;
        }
    }
}
