using MetaBlog.Domain.Follows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Infrastructure.Data.Configurations
{
    public class FollowConfiguration : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {
            builder.HasKey(f => new { f.FollowerId, f.FollowedId });
            builder.HasOne(f => f.Follower)
                   .WithMany(u => u.Following)
                   .HasForeignKey(f => f.FollowerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Followed)
                    .WithMany(u => u.Followers)
                    .HasForeignKey(f => f.FollowedId)
                    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
