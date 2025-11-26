using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using YeuBep.Entities;
using YeuBep.Extensions;

namespace YeuBep.Data.Interceptors;

public class AuditSaveChangeInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditSaveChangeInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        string[] createAuditColumns = [nameof(AuditEntity.CreatedById)];
        var context = eventData.Context;
        if (context == null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        var entries = context.ChangeTracker.Entries()
            .Where(e => e.Entity is AuditEntity &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));
        foreach (var entry in entries)
        {
            var auditEntity = (AuditEntity)entry.Entity;
            if (_httpContextAccessor.HttpContext is not null)
            {
                var currentUserId = _httpContextAccessor.HttpContext.GetUserId();
                if (currentUserId is not null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        auditEntity.CreatedById = currentUserId;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        auditEntity.ModifiedDate = DateTime.UtcNow;
                        auditEntity.ModifiedById = currentUserId;

                        foreach (var prop in createAuditColumns)
                        {
                            entry.Property(prop).IsModified = false;
                        }
                    }
                }
            }

        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}