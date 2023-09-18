using Microsoft.Extensions.DependencyInjection;
using WeatherService.Core;
using WeatherService.Testing.ServiceRegistrations.Specifications;

namespace WeatherService.Testing.ServiceRegistrations.Tests;

[TestFixture]
public class WeatherServiceCoreServiceCollectionTest : ServiceCollectionTestSpecification<AssemblyMarker>
{
    protected override void AddServices(ServiceCollection services)
    {
        services.AddWeatherServiceCore("", _ => { });
    }
}