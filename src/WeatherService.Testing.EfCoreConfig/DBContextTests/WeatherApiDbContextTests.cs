using WeatherService.Core.DatabaseConfiguration.DbContexts;
using WeatherService.Testing.EfCoreConfig.Specifications;

namespace WeatherService.Testing.EfCoreConfig.DBContextTests;

internal sealed class WeatherApiDbContextTests : DbContextTestSpecification<WeatherApiDbContext, Migrations.AssemblyMarker>
{
    /// <summary>
    /// Change datetime3 into datetime2 when needed for demo
    /// </summary>
    protected override string ErrorsToIgnore() =>
        """
        DIFFERENT: AuditLog->Property 'TimeStampUTC', column type. Expected = datetime3, found = datetime, but found True.
        """;
}