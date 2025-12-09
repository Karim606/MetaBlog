using MetaBlog.Domain.RefreshTokens;
using MetaBlog.Domain.Users;
using MetaBlog.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Infrastructure.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt=> rt.Id);
            builder.HasOne<User>().WithMany(u => u.refreshTokens).HasForeignKey(rt => rt.userId);
            builder.HasOne(rt => rt.ReplacedBy).WithOne().HasForeignKey<RefreshToken>(rt => rt.replacedByTokenId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
