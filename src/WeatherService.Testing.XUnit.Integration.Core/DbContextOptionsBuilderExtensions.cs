using Microsoft.EntityFrameworkCore;
using WeatherService.Testing.XUnit.Integration.Core.Infrastructure.Database;

namespace WeatherService.Testing.XUnit.Integration.Core;

public static class DbContextOptionsBuilderExtensions
{
    public static void UseTestDatabaseContext(this DbContextOptionsBuilder options, IDatabaseContext databaseContext) => databaseContext.Configure(options);
}