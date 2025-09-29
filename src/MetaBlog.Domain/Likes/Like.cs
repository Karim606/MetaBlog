using MetaBlog.Domain.Common;
using MetaBlog.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Domain.Likes
{
    public class Like:Entity
    {
        public Guid userId { get; }
        public Guid TargetId { get; set; }
        public LikeTargetType TargetType { get; set; }
        public DateTimeOffset likedAt { get; private set; }
        public User User { get; private set; }
        public bool IsDeleted { get; private set; }

        private Like() { }
        private Like(Guid id,Guid targetId,LikeTargetType targetType,Guid userId):base(id)
        {
       
            this.TargetId = targetId;
            this.userId = userId;
            likedAt = DateTimeOffset.UtcNow;
            TargetType = targetType;
        }
        public static Like Create(Guid id,Guid targetId,LikeTargetType targetType,Guid userId)
        {
            return new Like(id,targetId,targetType,userId);
        }


        public void SoftDelete() => IsDeleted = true;
        public void Restore() => IsDeleted = false;
    }
}
