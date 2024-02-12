using EfSchemaCompare;
using Xunit;
using WeatherService.Core.DatabaseConfiguration.DbContexts;
using WeatherService.Core.Features.AuditLogs.Models;
using WeatherService.Testing.XUnit.Integration.EfCoreConfig.Specifications;

namespace WeatherService.Testing.XUnit.Integration.EfCoreConfig.DBContextTests;

[Trait("DB", "LongRunning")]
public sealed class WeatherApiDbContextTests
    : DbContextTestSpecification<WeatherApiDbContext, Migrations.AssemblyMarker>
{
    /// <summary>
    /// Comment code in <see cref="AuditLogConfiguration"/> to see this test in action
    /// Change datetime3 into datetime2 when needed for demo
    /// </summary>
    protected override string ErrorsToIgnore() =>
        """
        DIFFERENT: AuditLog->Property 'TimeStampUTC', column type. Expected = datetime2, found = datetime
        """;
}