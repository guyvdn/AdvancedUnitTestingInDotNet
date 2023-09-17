using Microsoft.EntityFrameworkCore;
using WeatherService.Testing.Integration.Core.Infrastructure;

namespace WeatherService.Testing.Integration.Core;

public static class TestDatabaseContext
{
    //private static IDatabaseContext DatabaseContext { get; } = new LocalDbContext();
    private static IDatabaseContext DatabaseContext { get; } = new InMemoryDbContext();

    public static void Create() => DatabaseContext.CreateAsync();

    public static void Delete() => DatabaseContext.DeleteAsync();

    public static void UseTestDatabaseContext(this DbContextOptionsBuilder options) => DatabaseContext.Configure(options);
}