using MetaBlog.Domain.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Domain.RepositoriesInterfaces
{
    public interface ICommentRepository
    {
        public Task AddCommentAsync(Comment comment, CancellationToken cancellationToken);
        public Task DeleteCommentAsync(Comment comment, CancellationToken cancellationToken);
        public Task UpdateCommentAsync(Comment comment, CancellationToken cancellationToken);
        public Task<Comment> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task SaveChangesAsync(CancellationToken cancellationToken);


    }
}
