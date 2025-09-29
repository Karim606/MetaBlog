using MediatR;
using MetaBlog.Domain.Comments;
using MetaBlog.Domain.Common;
using MetaBlog.Domain.Favorites;
using MetaBlog.Domain.Likes;
using MetaBlog.Domain.Posts;
using MetaBlog.Domain.Users;
using MetaBlog.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<IdentityAppUser, IdentityRole<Guid>, Guid>
    {
        private readonly IMediator _mediator;
        public AppDbContext(DbContextOptions<AppDbContext> options,IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }
        protected override void OnModelCreating(ModelBuilder Builder)
        {
            base.OnModelCreating(Builder);
            Builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
        public DbSet<IdentityAppUser> IdentityAppUsers { get; set; }
        public new DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Favorite> Favorites { get; set; }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await DispatchDomainEventsAsync(cancellationToken);
            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task DispatchDomainEventsAsync(CancellationToken cancellationToken = default)
        {
            var domainEntities = ChangeTracker
                .Entries()
                .Where(x => x.Entity is Entity baseEntity && baseEntity.domainEvents.Any())
                .Select(x => (Entity)x.Entity)
                .ToList();  
            var domainEvents = domainEntities
                .SelectMany(x => x.domainEvents)
                .ToList();

            
            foreach (var domainEvent in domainEvents)
            {
               await _mediator.Publish(domainEvent, cancellationToken);
            }
            domainEntities.ForEach(entity => entity.ClearDomainEvents());
        }
    }
}
