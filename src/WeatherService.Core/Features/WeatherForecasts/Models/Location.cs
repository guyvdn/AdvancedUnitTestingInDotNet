namespace WeatherService.Core.Features.WeatherForecasts.Models;

public class Location
{
    public required string Name { get; set; }

    public required string Region { get; set; }

    public required string Country { get; set; }
}