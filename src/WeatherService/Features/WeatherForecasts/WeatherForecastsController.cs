using WeatherService.Core.Features.WeatherForecasts;

namespace WeatherService.Api.Features.WeatherForecast;

[Authorize]
[ApiController]
[Route("[controller]")]
public sealed class WeatherForecastsController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    private readonly ISender _sender;

    public WeatherForecastsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<Representation.WeatherForecast> Get(string city, CancellationToken cancellationToken)
    {
        var request = new GetCurrentWeather.Request
        {
            City = city
        };

        var foreCast = await _sender.Send(request, cancellationToken);
        return foreCast.ToRepresentation();
    }
}