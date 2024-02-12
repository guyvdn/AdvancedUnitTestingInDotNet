using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

[assembly: InternalsVisibleTo("WeatherService.Testing.NUnit.Unit.Core")]
[assembly: InternalsVisibleTo("WeatherService.Testing.NUnit.Unit.Features")]
[assembly: InternalsVisibleTo("WeatherService.Testing.NUnit.Integration.Core")]
[assembly: InternalsVisibleTo("WeatherService.Testing.NUnit.Integration.Features")]
[assembly: InternalsVisibleTo("WeatherService.Testing.NUnit.Integration.EfCoreConfig")]
[assembly: InternalsVisibleTo("WeatherService.Testing.NUnit.Integration.Seeding")]

[assembly: InternalsVisibleTo("WeatherService.Testing.XUnit.Unit.Core")]
[assembly: InternalsVisibleTo("WeatherService.Testing.XUnit.Unit.Features")]
[assembly: InternalsVisibleTo("WeatherService.Testing.XUnit.Integration.Core")]
[assembly: InternalsVisibleTo("WeatherService.Testing.XUnit.Integration.Features")]
[assembly: InternalsVisibleTo("WeatherService.Testing.XUnit.Integration.EfCoreConfig")]
[assembly: InternalsVisibleTo("WeatherService.Testing.XUnit.Integration.Seeding")]

namespace WeatherService.Core;

public struct AssemblyMarker
{
}