using WeatherService.Core.DatabaseConfiguration.DbContexts;
using WeatherService.Core.Features.AuditLog;

namespace WeatherService.Core;

internal interface IWeatherRepository
{
    Task AddAuditLogAsync(AuditLog auditLog, CancellationToken cancellationToken);
}

internal sealed class WeatherRepository: IWeatherRepository
{
    private readonly WeatherApiDbContext _dbContext;

    public WeatherRepository(WeatherApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAuditLogAsync(AuditLog auditLog, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(auditLog, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}