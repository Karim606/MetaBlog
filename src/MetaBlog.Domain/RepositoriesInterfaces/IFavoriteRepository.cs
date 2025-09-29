using MetaBlog.Domain.Favorites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Domain.RepositoriesInterfaces
{
    public interface IFavoriteRepository
    {
        public Task AddFavorite(Favorite favorite); 
        public Task RemoveFavorite(Favorite favorite);
        public Task<Favorite> GetFavorite(Guid id);
        public  Task<bool> FavoriteExist(Guid postId, Guid userId);
        public  Task SaveChangesAsync();
    }
}
