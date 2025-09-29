using MetaBlog.Domain.Likes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Infrastructure.Data.Configurations
{
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.HasKey(l => l.Id);
            builder.Property(l => l.likedAt).IsRequired();
            builder.Property(l => l.TargetId).IsRequired();
            builder.Property(l => l.TargetType).IsRequired();
            builder.Property(l => l.userId).IsRequired();
            builder.HasIndex(l => new { l.userId, l.TargetId, l.TargetType }).IsUnique();
          
        }
    }
}
