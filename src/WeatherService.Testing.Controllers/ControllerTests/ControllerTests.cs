using WeatherService.Api.Features.AuditLogs;
using WeatherService.Api.Features.Images;
using WeatherService.Api.Features.WeatherForecast;
using WeatherService.Testing.Controllers.Specifications;

namespace WeatherService.Testing.Controllers.ControllerTests;

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