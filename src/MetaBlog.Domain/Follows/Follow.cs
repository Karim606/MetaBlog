using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Domain.Follows
{
    public class Follow
    {
        public Guid FollowerId { get; set; }
        public Guid FollowedId { get; set; }
        private Follow() { }
        private Follow(Guid FollowerId,Guid FollowedId) {
     
            this.FollowerId = FollowerId;
            this.FollowedId = FollowedId;
        }
        public DateTime FollowedAt { get; set; }= DateTime.UtcNow;
        public User Follower { get; set; }
        public User Followed { get; set; }

        public static Result<Follow> Create(Guid FollowerId, Guid FollowedId)
        {
            if (FollowerId == FollowedId)
            {
                return Error.Conflict(description:"A user cannot follow himself.");
            }

            var follow = new Follow(FollowerId, FollowedId);
            return follow;
        }
    }
}
