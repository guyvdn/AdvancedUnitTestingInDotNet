namespace WeatherService.Api;

public class WeatherForecast
{
    public required DateTime Date { get; set; }

    public required double TemperatureC { get; set; }

    public double TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}