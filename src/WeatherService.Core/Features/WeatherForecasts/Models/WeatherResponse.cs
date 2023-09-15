namespace WeatherService.Core.Features.WeatherForecasts.Models;

public class WeatherResponse
{
    public required Location Location { get; set; }

    public required CurrentWeather Current { get; set; }
}