using System.ComponentModel.DataAnnotations;

namespace WeatherService.Api.Configuration;

internal sealed class AuthenticationConfigurationOptions
{
    [Required] 
    public string Authority { get; set; } = default!;

    [Required]
    public string Audience { get; set; } = default!;
}