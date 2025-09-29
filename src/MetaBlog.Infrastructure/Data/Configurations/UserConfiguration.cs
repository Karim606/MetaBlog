using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaBlog.Domain.Users;
using MetaBlog.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace MetaBlog.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Bio).HasMaxLength(200);
            builder.Property(u => u.imageUrl).HasMaxLength(200);
            builder.Property(u => u.isActive).HasDefaultValue(true);
            builder.HasMany(u => u.Posts).WithOne(p => p.User).HasForeignKey(p => p.authorId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(u => u.Likes).WithOne(l => l.User).HasForeignKey(l => l.userId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(u => u.Comments).WithOne(c => c.User).HasForeignKey(c => c.userId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(u => u.Favorites).WithOne().HasForeignKey(f => f.userId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<IdentityAppUser>().WithOne().HasForeignKey<User>(u => u.Id).OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(u => u.Comments).UsePropertyAccessMode(PropertyAccessMode.Field);
            builder.Navigation(u => u.Favorites).UsePropertyAccessMode(PropertyAccessMode.Field);
            builder.Navigation(u => u.Likes).UsePropertyAccessMode(PropertyAccessMode.Field);
            builder.Navigation(u => u.Posts).UsePropertyAccessMode(PropertyAccessMode.Field);




        }
    }
}
