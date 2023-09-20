using WeatherService.Core.Features.WeatherForecasts;

namespace WeatherService.Api.Features.WeatherForecasts;

[Authorize]
[ApiController]
[Route("[controller]")]
public sealed class WeatherForecastsController : ControllerBase
{
    private readonly ISender _sender;

    public WeatherForecastsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    [ProducesResponseType(typeof(Representation.WeatherForecast), Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), Status400BadRequest)]
    public async Task<IActionResult> Get(
        string city = "",
        CancellationToken cancellationToken = default)
    {
        var request = new GetCurrentWeather.Request
        {
            City = city
        };

        var foreCast = await _sender.Send(request, cancellationToken);
        return Ok(foreCast.ToRepresentation());
    }
}

[ApiController]
[Route("[controller]")]
internal class BadWeatherForecastsController : ControllerBase
{
    private readonly ISender _sender;

    public BadWeatherForecastsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<Representation.WeatherForecast> Get(
        string city = "",
        CancellationToken cancellationToken = default)
    {
        var request = new GetCurrentWeather.Request
        {
            City = city
        };

        var foreCast = await _sender.Send(request, cancellationToken);
        return foreCast.ToRepresentation();
    }
}