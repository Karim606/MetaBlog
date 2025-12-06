using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Follow.Dtos.response;
using MetaBlog.Domain.Follows;
namespace MetaBlog.Application.Features.Follow
{
    public interface IFollowQueryService
    {
        Task<PaginatedList<Followers_FollowedDto>> GetFollowsAsync(Guid userId, FollowQueryType queryType, int pageNumber, int pageSize, string? searchTerm,
        DateTime? createdAfter, bool? sortDescending, CancellationToken ct);

        Task<(int totalFollowers, int totalFollowed)> GetTotalNumberAsync(Guid id, CancellationToken ct);

        Task<bool> Followed(Guid id,Guid followedId, CancellationToken ct);
    }
}
