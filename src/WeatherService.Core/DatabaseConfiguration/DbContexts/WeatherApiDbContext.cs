using Microsoft.EntityFrameworkCore;
using WeatherService.Core.Features.AuditLogs.Models;

namespace WeatherService.Core.DatabaseConfiguration.DbContexts;

internal class WeatherApiDbContext : DbContext
{
    public WeatherApiDbContext(DbContextOptions<WeatherApiDbContext> options) : base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyMarker).Assembly);
    }
}