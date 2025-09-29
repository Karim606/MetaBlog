using MetaBlog.Domain.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Domain.RepositoriesInterfaces
{
    public interface IPostRepository
    {
        public Task<Post> GetPostBySlugAsync(string slug, CancellationToken cancellationToken);
        public Task<Post> GetPostByIDAsync(Guid id, CancellationToken cancellationToken);
        public Task AddPostAsync(Post post, CancellationToken cancellationToken);
        public Task<int> GetTotalPostsCountByAuthorIDAsync(Guid id,CancellationToken cancellationToken);
        public Task SaveChangesAsync(CancellationToken cancellationToken);
        public Task EditPostAsync(Post post, CancellationToken cancellationToken);
        public Task DeletePostAsync(Post post, CancellationToken cancellationToken);

    }
}
