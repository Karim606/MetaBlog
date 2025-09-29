using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
namespace MetaBlog.Infrastructure.Data.Interceptors
{
    public class AuditableEntityInterceptor( ICurrentUserService user,Timer timer): SaveChangesInterceptor
    {
        private readonly ICurrentUserService _user = user;
        private readonly Timer _timer = timer;
        
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData,InterceptionResult<int>result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);   
        }
        public override  ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,CancellationToken ct)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result);
        }

        public void UpdateEntities(DbContext? context)
        {
            if (context == null) return;
            var entries = context.ChangeTracker.Entries<AuditableEntity>();
            foreach (var entry in entries)
            {
                if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    var datetime = DateTimeOffset.UtcNow;
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.createdBy = _user.GetId();
                        entry.Entity.createdAt = datetime;
                    }

                    entry.Entity.lastModifiedBy = _user.GetId();
                    entry.Entity.lastModifiedAt = datetime;
                    foreach (var ownedEntry in entry.References)
                    {
                        var target = ownedEntry.TargetEntry;

                        if (target is not { Entity: AuditableEntity ownedEntity } ||
                            target.State is not (EntityState.Added or EntityState.Modified))
                        {
                            continue; // skip irrelevant entries
                        }

                        if (target.State == EntityState.Added)
                        {
                            ownedEntity.createdBy = _user.GetId();
                            ownedEntity.createdAt = datetime;
                        }

                        ownedEntity.lastModifiedBy = _user.GetId();
                        ownedEntity.lastModifiedAt = datetime;
                    }
                }
            }
        }

    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry)
    {
        return entry.References.Any(r => r.TargetEntry != null && r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified || r.TargetEntry.HasChangedOwnedEntities()));
    }
}