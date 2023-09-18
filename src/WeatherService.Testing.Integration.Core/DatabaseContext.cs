using WeatherService.Testing.Integration.Core.Infrastructure.Database;

namespace WeatherService.Testing.Integration.Core;

public static class DatabaseContext
{
    public static IDatabaseContext Current { get; set; } = new InMemoryDbContext();
    //public static IDatabaseContext Current { get; } = new LocalDbContext();
    //public static IDatabaseContext Current { get; } = new SqlDockerDbContext();
}