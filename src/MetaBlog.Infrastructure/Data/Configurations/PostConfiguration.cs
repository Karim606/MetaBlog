using MetaBlog.Domain.Posts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaBlog.Domain.Likes;
using MetaBlog.Domain.Comments;
using MetaBlog.Domain.Favorites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace MetaBlog.Infrastructure.Data.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
           builder.HasKey(p => p.Id);
           builder.Property(p => p.Title).IsRequired().HasMaxLength(200);
           builder.Property(p => p.Content).IsRequired();
           builder.Property(p => p.Slug).IsRequired().HasMaxLength(250);
           builder.Property(p => p.authorId).IsRequired();
           builder.HasMany<Comment>(p => p.Comments).WithOne().HasForeignKey(c => c.postId).OnDelete(DeleteBehavior.Cascade);
           builder.HasMany<Favorite>().WithOne(f => f.post).HasForeignKey(f => f.postId).OnDelete(DeleteBehavior.NoAction);
           builder.Navigation(p => p.Comments).UsePropertyAccessMode(PropertyAccessMode.Field);

        }
    }
}
