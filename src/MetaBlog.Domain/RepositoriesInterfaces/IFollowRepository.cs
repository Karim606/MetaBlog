using MetaBlog.Domain.Follows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Domain.RepositoriesInterfaces
{
    public interface IFollowRepository
    {
        Task FollowUserAsync(Follow followRequest);
        Task UnfollowUserAsync(Follow unfollowRequest);
        Task<Follow> GetFollowAsync(Guid followerId, Guid followedId);

        Task SaveChangesAsync();
    }
}
