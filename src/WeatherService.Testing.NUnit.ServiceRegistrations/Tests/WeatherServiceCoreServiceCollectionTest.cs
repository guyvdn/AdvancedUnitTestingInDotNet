using Microsoft.Extensions.DependencyInjection;
using WeatherService.Core;
using WeatherService.Testing.NUnit.ServiceRegistrations.Specifications;

namespace WeatherService.Testing.NUnit.ServiceRegistrations.Tests;

[TestFixture]
public class WeatherServiceCoreServiceCollectionTest : ServiceCollectionTestSpecification<AssemblyMarker>
{
    protected override void AddServices(ServiceCollection services)
    {
        services.AddWeatherServiceCore("", _ => { });
    }
}