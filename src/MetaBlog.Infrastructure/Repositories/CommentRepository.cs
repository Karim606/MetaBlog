using MetaBlog.Domain.Comments;
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
    public class CommentRepository(AppDbContext appDbContext) : ICommentRepository
    {
        public async Task AddCommentAsync(Comment comment, CancellationToken cancellationToken)
        {
            await appDbContext.Comments.AddAsync(comment);
            await SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteCommentAsync(Comment comment, CancellationToken cancellationToken)
        {
            appDbContext.Comments.Remove(comment);
            await SaveChangesAsync(cancellationToken);
        }

        public async Task<Comment> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await appDbContext.Comments.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }


        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await appDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateCommentAsync(Comment comment, CancellationToken cancellationToken)
        {
            appDbContext.Update(comment);
            await SaveChangesAsync(cancellationToken);
        }
    }
}
