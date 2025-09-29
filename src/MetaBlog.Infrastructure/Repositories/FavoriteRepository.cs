using MetaBlog.Domain.Favorites;
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
    public class FavoriteRepository(AppDbContext context): IFavoriteRepository
    {
        public async Task AddFavorite(Favorite favorite)
        {
            context.Favorites.Add(favorite);
            await context.SaveChangesAsync();
        }

        public async Task RemoveFavorite(Favorite favorite)
        {
            context.Favorites.Remove(favorite);
            await SaveChangesAsync();
        }
        public async Task<bool> FavoriteExist(Guid postId,Guid userId)
        {
            return await context.Favorites.AnyAsync(f => f.postId == postId && f.userId == userId); 
        }
        public async Task<Favorite> GetFavorite(Guid id)
        {
            return await context.Favorites.FirstOrDefaultAsync(f => f.Id == id);
        }
        public async Task SaveChangesAsync()
        {
             await context.SaveChangesAsync();
        }
    }
}
