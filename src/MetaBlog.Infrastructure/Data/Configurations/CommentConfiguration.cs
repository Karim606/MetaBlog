using MetaBlog.Domain.Comments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Infrastructure.Data.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.parentCommentId).IsRequired(false);
            builder.Property(c => c.content).IsRequired();
            builder.Property(c => c.postId).IsRequired();
            builder.Property(c => c.userId).IsRequired();
            builder.HasMany(c => c.Replies).WithOne().HasForeignKey(c=> c.parentCommentId).OnDelete(DeleteBehavior.NoAction);
            
            builder.Navigation(c => c.Replies).UsePropertyAccessMode(PropertyAccessMode.Field);

        }
    }
}
