using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

[assembly: InternalsVisibleTo("WeatherService.Testing.Core")]
[assembly: InternalsVisibleTo("WeatherService.Testing.Features")]
[assembly: InternalsVisibleTo("WeatherService.Testing.EfCoreConfig")]

[assembly: InternalsVisibleTo("WeatherService.Testing.Integration.Core")]
[assembly: InternalsVisibleTo("WeatherService.Testing.Integration.Seeding")]

namespace WeatherService.Core;

public struct AssemblyMarker
{
}