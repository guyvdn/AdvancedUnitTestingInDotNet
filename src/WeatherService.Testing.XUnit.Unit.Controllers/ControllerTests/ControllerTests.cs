using WeatherService.Api.Features.AuditLogs;
using WeatherService.Api.Features.Images;
using WeatherService.Api.Features.WeatherForecasts;
using WeatherService.Testing.XUnit.Unit.Controllers.Specifications;

namespace WeatherService.Testing.XUnit.Unit.Controllers.ControllerTests;

public sealed class WeatherForecastControllerTest 
    : ControllerTestSpecification<WeatherForecastsController>
{
}

public sealed class AuditLogsControllerTest 
    : ControllerTestSpecification<AuditLogsController>
{
}

public sealed class ImagesControllerTest 
    : ControllerTestSpecification<ImagesController>
{
}