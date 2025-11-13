using MediatR;
using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Follow.Dtos.response;
using MetaBlog.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Follow.Queries.GetFollowers
{
    public record GetFollowersQuery(Guid userId,int pageNumber=1, int pageSize=10, string? searchTerm=null, DateTime? createdAfter = null, bool? sortDescending = null)
        :IRequest<Result<PaginatedList<Followers_FollowedDto>>>;
  
}
