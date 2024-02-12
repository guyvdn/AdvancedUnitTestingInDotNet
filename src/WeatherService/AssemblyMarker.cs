using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("WeatherService.Testing.NUnit.Unit.Core")]
[assembly: InternalsVisibleTo("WeatherService.Testing.NUnit.Unit.Features")]
[assembly: InternalsVisibleTo("WeatherService.Testing.NUnit.Unit.Controllers")]
[assembly: InternalsVisibleTo("WeatherService.Testing.NUnit.Integration.Core")]
[assembly: InternalsVisibleTo("WeatherService.Testing.NUnit.Integration.Features")]

namespace WeatherService.Api;

public sealed class AssemblyMarker
{
}