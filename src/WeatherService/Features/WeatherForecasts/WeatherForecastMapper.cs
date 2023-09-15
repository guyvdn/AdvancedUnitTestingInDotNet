using WeatherService.Core.Features.WeatherForecasts.Models;

namespace WeatherService.Api.Features.WeatherForecast;

internal static class WeatherForecastMapper
{
    public static Representation.WeatherForecast ToRepresentation(this WeatherResponse foreCast)
    { 
        return new Representation.WeatherForecast
        {
            TemperatureC = foreCast.Current.TemperatureC,
            TemperatureF = foreCast.Current.TemperatureF,
            Summary = foreCast.Current.Condition.Text,
            LastUpdated = DateTime.Parse(foreCast.Current.LastUpdated),
        };
    }
}