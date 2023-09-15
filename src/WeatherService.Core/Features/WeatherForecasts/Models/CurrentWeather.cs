using System.Text.Json.Serialization;

namespace WeatherService.Core.Features.WeatherForecasts.Models;

public class CurrentWeather
{
    [JsonPropertyName("temp_c")]
    public required double TemperatureC { get; set; }

    public double TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public required Condition Condition { get; set; }

    [JsonPropertyName("last_updated")]
    public required string LastUpdated { get; set; }
}