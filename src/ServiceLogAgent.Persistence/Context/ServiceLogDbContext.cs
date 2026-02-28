using Microsoft.EntityFrameworkCore;
using ServiceLogAgent.Domain.Entities;

namespace ServiceLogAgent.Persistence.Context;

public class ServiceLogDbContext(DbContextOptions<ServiceLogDbContext> options) : DbContext(options)
{
    public DbSet<ServiceLog> ServiceLogs => Set<ServiceLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ServiceLog>();

        entity.HasKey(x => x.Id);
        entity.Property(x => x.CorrelationId).HasMaxLength(128);
        entity.Property(x => x.TraceId).HasMaxLength(256);
        entity.Property(x => x.HttpMethod).HasMaxLength(16);
        entity.Property(x => x.Path).HasMaxLength(2048);
        entity.Property(x => x.QueryString).HasMaxLength(2048);
        entity.Property(x => x.RemoteIp).HasMaxLength(128);
        entity.Property(x => x.UserId).HasMaxLength(256);
        entity.Property(x => x.ApplicationKey).HasMaxLength(256);
        entity.Property(x => x.ErrorMessage).HasMaxLength(4096);

        entity.HasIndex(x => x.CreatedAtUtc);
        entity.HasIndex(x => x.ResponseStatusCode);
    }
}
