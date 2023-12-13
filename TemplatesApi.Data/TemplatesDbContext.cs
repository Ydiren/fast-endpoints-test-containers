using Microsoft.EntityFrameworkCore;
using TemplatesApi.Data.Entities;

namespace TemplatesApi.Data;

public class TemplatesDbContext : DbContext
{
    public DbSet<Template> Templates { get; set; } = default!;
    
    public TemplatesDbContext(DbContextOptions<TemplatesDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        UpdateAuditDetails();

        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateAuditDetails();
        
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateAuditDetails();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateAuditDetails();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void UpdateAuditDetails()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IAuditable && (
                e.State == EntityState.Added
                || e.State == EntityState.Modified))
            .Select(e => (IAuditable)e.Entity);

        foreach (var entry in entries)
        {
            if (entry.CreatedAt == default)
            {
                entry.CreatedAt = DateTimeOffset.UtcNow;
            }
            else
            {
                entry.ModifiedAt = DateTimeOffset.UtcNow;
            }
        }
    }
}