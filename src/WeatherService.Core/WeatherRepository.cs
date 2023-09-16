using Microsoft.EntityFrameworkCore;
using WeatherService.Core.DatabaseConfiguration.DbContexts;
using WeatherService.Core.Features.AuditLogs.Models;

namespace WeatherService.Core;

internal interface IWeatherRepository
{
    Task AddAuditLogAsync(AuditLog auditLog, CancellationToken cancellationToken);

    Task<List<AuditLog>> GetAuditLogsAsync(int numberOfDays, CancellationToken cancellationToken);
}

internal sealed class WeatherRepository: IWeatherRepository
{
    private readonly IDbContextFactory<WeatherApiDbContext> _dbContextFactory;

    public WeatherRepository(IDbContextFactory<WeatherApiDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task AddAuditLogAsync(AuditLog auditLog, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        await dbContext.AddAsync(auditLog, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<AuditLog>> GetAuditLogsAsync(int numberOfDays, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await dbContext.AuditLogs
            .Where(x => x.TimeStampUTC >= DateTime.UtcNow.Date.AddDays(-numberOfDays))
            .ToListAsync(cancellationToken);
    }
}