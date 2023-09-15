namespace WeatherService.Representation;

[Serializable]
public class WeatherForecast
{
    public required double TemperatureC { get; set; }

    public required double TemperatureF { get; set; }
    
    public string? Summary { get; set; }

    public DateTime LastUpdated { get; set; }
}