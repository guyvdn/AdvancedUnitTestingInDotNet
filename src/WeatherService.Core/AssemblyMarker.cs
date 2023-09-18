using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("WeatherService.Testing.Unit.Core")]
[assembly: InternalsVisibleTo("WeatherService.Testing.Unit.Features")]
[assembly: InternalsVisibleTo("WeatherService.Testing.Integration.Core")]
[assembly: InternalsVisibleTo("WeatherService.Testing.Integration.EfCoreConfig")]
[assembly: InternalsVisibleTo("WeatherService.Testing.Integration.Seeding")]

namespace WeatherService.Core;

public struct AssemblyMarker
{
}