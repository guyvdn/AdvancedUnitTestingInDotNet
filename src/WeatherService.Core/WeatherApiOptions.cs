namespace WeatherService.Core;

public class WeatherApiOptions
{
    [Required]
    public string ApiKey { get; set; } = default!;
}