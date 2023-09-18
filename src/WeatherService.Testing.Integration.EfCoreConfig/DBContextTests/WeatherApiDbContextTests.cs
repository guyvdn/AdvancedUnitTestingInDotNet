using NUnit.Framework;
using WeatherService.Core.DatabaseConfiguration.DbContexts;
using WeatherService.Core.Features.AuditLogs.Models;
using WeatherService.Testing.Integration.EfCoreConfig.Specifications;

namespace WeatherService.Testing.Integration.EfCoreConfig.DBContextTests;

[Explicit("LongRunning")]
[Category("LongRunning")]
internal sealed class WeatherApiDbContextTests
    : DbContextTestSpecification<WeatherApiDbContext, Migrations.AssemblyMarker>
{
    /// <summary>
    /// Comment code in <see cref="AuditLogConfiguration"/> to see this test in action
    /// Change datetime3 into datetime2 when needed for demo
    /// </summary>
    protected override string ErrorsToIgnore() =>
        """
        DIFFERENT: AuditLog->Property 'TimeStampUTC', column type. Expected = datetime3, found = datetime
        """;
}