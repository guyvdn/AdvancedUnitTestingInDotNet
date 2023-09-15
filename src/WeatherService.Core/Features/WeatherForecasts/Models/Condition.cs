namespace WeatherService.Core.Features.WeatherForecasts.Models;

public class Condition
{
    public required string Text { get; set; }

    public required int Code { get; set; }
}