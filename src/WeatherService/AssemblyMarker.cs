using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("WeatherService.Testing.Controllers")]
[assembly: InternalsVisibleTo("WeatherService.Testing.Integration.Core")]

namespace WeatherService.Api;

public sealed class AssemblyMarker
{
}