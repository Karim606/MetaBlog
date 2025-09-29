using MetaBlog.Domain.Common.Interfaces;
using MetaBlog.Domain.Likes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Domain.RepositoriesInterfaces
{
    public interface ILikeRepository
    {
        public Task AddLikeAsync<T>(T entity, Guid targetId, LikeTargetType targetType, Guid userId) 
            where T : class,ILikable;
      //  public bool AlreadyLikedAsync(Guid targetId,LikeTargetType targetType,Guid userId);
        public Task RemoveLikeAsync<T>(T entity,Guid targetId, LikeTargetType targetType, Guid userId)
            where T : class, ILikable;
        public Task<Like> GetLikeByIdAsync(Guid id);
        public Task SaveChangesAsync();
    }
}
