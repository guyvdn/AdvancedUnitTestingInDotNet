using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherService.Core.Features.CurrentWeather;

namespace WeatherService.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherApiClient _weatherApiClient;

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ISender _sender;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(
        ISender sender,
        ILogger<WeatherForecastController> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<WeatherForecast> Get(string? city, CancellationToken cancellationToken)
    {
        var request = new GetCurrentWeather.Request
        {
            City = city
        };

        var foreCast = await _sender.Send(request, cancellationToken);
        
        return new WeatherForecast
        {
            Date = DateTime.Now,
            TemperatureC = foreCast.Current.Temperature,
        };
    }
}