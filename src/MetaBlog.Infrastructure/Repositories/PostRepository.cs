using MetaBlog.Domain.RepositoriesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaBlog.Domain.Posts;
using MetaBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MetaBlog.Infrastructure.Repositories
{
    public class PostRepository(AppDbContext appDbContext) : IPostRepository
    {
        public async Task AddPostAsync(Post post, CancellationToken cancellationToken)
        {
            await appDbContext.Posts.AddAsync(post);
            await SaveChangesAsync(cancellationToken);
        }

        public async Task DeletePostAsync(Post post, CancellationToken cancellationToken)
        {
             appDbContext.Posts.Remove(post);
            await SaveChangesAsync(cancellationToken);
        }

        public async Task EditPostAsync(Post post, CancellationToken cancellationToken)
        {
            appDbContext.Posts.Update(post);
            await SaveChangesAsync(cancellationToken);
        }

        public async Task<Post> GetPostByIDAsync(Guid id, CancellationToken cancellationToken)
        {
            return  await appDbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Post> GetPostBySlugAsync(string slug, CancellationToken cancellationToken)
        {
           return await appDbContext.Posts.FirstOrDefaultAsync(p => p.Slug == slug);

        }

        public Task<int> GetTotalPostsCountByAuthorIDAsync(Guid id, CancellationToken cancellationToken)
        {
            return appDbContext.Posts.Where(p => p.authorId == id).CountAsync();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
           await appDbContext.SaveChangesAsync(cancellationToken);  
        }
    }
}
