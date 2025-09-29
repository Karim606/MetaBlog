using MetaBlog.Domain.Common.Interfaces;
using MetaBlog.Domain.Likes;
using MetaBlog.Domain.RepositoriesInterfaces;
using MetaBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Infrastructure.Repositories
{
    public class LikeRepository(AppDbContext context) : ILikeRepository
    {
        public async Task AddLikeAsync<T>(T entity,Guid targetId, LikeTargetType targetType, Guid userId)
            where T : class, ILikable
        {
           using var tx = await context.Database.BeginTransactionAsync();
            try
            {
                var like = await context.Likes.FirstOrDefaultAsync(l => l.TargetId == targetId && l.TargetType == targetType && l.userId == userId);
                if (like == null)
                {
                    like = Like.Create(new Guid(), targetId, targetType, userId);
                    await context.Likes.AddAsync(like);
                }
                else if (like.IsDeleted)
                {
                    like.Restore();
                }
                entity.IncrementLike();
                await SaveChangesAsync();
                tx.Commit();
            }
            catch (Exception ex) { 
             await tx.RollbackAsync();
                throw ex;
            }
        }

        public async Task<bool> AlreadyLikedAsync(Guid targetId, LikeTargetType targetType, Guid userId)
        {
            return await context.Likes.AnyAsync(l=> l.TargetId==targetId && l.TargetType==targetType &&l.userId==userId);
        }

        public Task<Like> GetLikeByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

       public async Task RemoveLikeAsync<T>(T entity, Guid targetId, LikeTargetType targetType, Guid userId)
             where T : class, ILikable
        {
            using var tx = await context.Database.BeginTransactionAsync();

            try
            {
                var like = await context.Likes
                    .FirstOrDefaultAsync(l => l.TargetType == targetType
                                           && l.TargetId == entity.Id
                                           && l.userId == userId
                                           && !l.IsDeleted);

                if (like != null)
                {
                    like.SoftDelete();
                    entity.DecrementLike();

                    await context.SaveChangesAsync();
                    await tx.CommitAsync();
                }
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
    }
}
