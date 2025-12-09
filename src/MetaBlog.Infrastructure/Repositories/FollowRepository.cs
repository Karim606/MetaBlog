using MetaBlog.Domain.Follows;
using MetaBlog.Domain.RepositoriesInterfaces;
using MetaBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Infrastructure.Repositories
{
    public class FollowRepository(AppDbContext context) : IFollowRepository
    {
        public async Task FollowUserAsync(Follow followRequest)
        {
           await context.Follows.AddAsync(followRequest);
           await SaveChangesAsync();
        }

        public async Task<Follow> GetFollowAsync(Guid followerId, Guid followedId)
        {
          return await context.Follows.FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowedId == followedId);
        }

        public async Task UnfollowUserAsync(Follow unfollowRequest)
        {
            context.Remove(unfollowRequest);
            await SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
           await context.SaveChangesAsync();
        }
    }
}
