using Microsoft.Extensions.DependencyInjection;
using WeatherService.Core;
using WeatherService.Testing.XUnit.ServiceRegistrations.Specifications;

namespace WeatherService.Testing.XUnit.ServiceRegistrations.Tests;

public class WeatherServiceCoreServiceCollectionTest : ServiceCollectionTestSpecification<AssemblyMarker>
{
    protected override void AddServices(ServiceCollection services)
    {
        services.AddWeatherServiceCore("", _ => { });
    }
}