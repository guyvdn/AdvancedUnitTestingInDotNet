namespace WeatherService.Core;

public class WeatherApiOptions
{
    [Required]
    public string ApiUrl { get; set; } = default!;

    [Required]
    public string ApiKey { get; set; } = default!;
}