using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("WeatherService.Testing.Unit.Core")]
[assembly: InternalsVisibleTo("WeatherService.Testing.Unit.Features")]
[assembly: InternalsVisibleTo("WeatherService.Testing.Unit.Controllers")]
[assembly: InternalsVisibleTo("WeatherService.Testing.Integration.Core")]
[assembly: InternalsVisibleTo("WeatherService.Testing.Integration.Features")]

namespace WeatherService.Api;

public sealed class AssemblyMarker
{
}