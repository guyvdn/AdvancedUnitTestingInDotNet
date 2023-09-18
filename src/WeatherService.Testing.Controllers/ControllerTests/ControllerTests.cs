using WeatherService.Api.Features.AuditLogs;
using WeatherService.Api.Features.Images;
using WeatherService.Api.Features.WeatherForecasts;
using WeatherService.Testing.Unit.Controllers.Specifications;

namespace WeatherService.Testing.Unit.Controllers.ControllerTests;

internal sealed class WeatherForecastControllerTest 
    : ControllerTestSpecification<WeatherForecastsController>
{
}

internal sealed class AuditLogsControllerTest 
    : ControllerTestSpecification<AuditLogsController>
{
}

internal sealed class ImagesControllerTest 
    : ControllerTestSpecification<ImagesController>
{
}