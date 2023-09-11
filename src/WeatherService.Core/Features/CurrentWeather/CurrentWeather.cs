using System.Text.Json.Serialization;

namespace WeatherService.Core.Features.CurrentWeather;

public class WeatherResponse
{
    public Location Location { get; set; }

    public CurrentWeather Current { get; set; }
}

public class Location
{
    public string Name { get; set; }

    public string Region { get; set; }

    public string Country { get; set; }
}

public class CurrentWeather
{
    [JsonPropertyName("temp_c")]
    public double Temperature { get; set; }
}

