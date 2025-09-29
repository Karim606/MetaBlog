using MetaBlog.Domain.Common;
using MetaBlog.Domain.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Domain.Favorites
{
    public class Favorite:Entity
    {
        public Guid postId { get; }
        public Guid userId { get; }
        public DateTimeOffset favoritedAt { get; private set; }
        public Post post { get; private set; }
        private Favorite() { }
        private Favorite(Guid id,Guid postId,Guid userId):base(id) 
        {
            this.postId = postId;
            this.userId = userId;
            favoritedAt = DateTimeOffset.UtcNow;
        }

        public static Favorite Create(Guid id,Guid postId,Guid userId)
        {
            return new Favorite(id,postId,userId);
        }
    }
}
