using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WeatherService.Core.DatabaseConfiguration.DbContexts;

namespace WeatherService.Core;

public static class ServiceCollectionExtensions
{
    public static void AddWeatherServiceCore(this IServiceCollection services, string connectionString, Action<WeatherApiOptions> weatherApiOptions)
    {
        services
            .AddOptions<WeatherApiOptions>()
            .Configure(weatherApiOptions)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddDbContext<WeatherApiDbContext>(o => o.UseSqlServer(connectionString));

        services.TryAddScoped<IWeatherRepository, WeatherRepository>();

        services.AddHttpClient<IWeatherApiClient, WeatherApiClient>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<AssemblyMarker>();
        });

        services.AddHttpContextAccessor();
    }
}