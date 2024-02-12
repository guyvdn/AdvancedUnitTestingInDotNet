using Microsoft.EntityFrameworkCore;
using WeatherService.Testing.NUnit.Integration.Core.Infrastructure.Database;

namespace WeatherService.Testing.NUnit.Integration.Core;

public static class DbContextOptionsBuilderExtensions
{
    public static void UseTestDatabaseContext(this DbContextOptionsBuilder options, IDatabaseContext databaseContext) => databaseContext.Configure(options);
}