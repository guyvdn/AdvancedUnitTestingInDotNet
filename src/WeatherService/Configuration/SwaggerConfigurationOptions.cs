using System.ComponentModel.DataAnnotations;

namespace WeatherService.Api.Configuration;

internal sealed class SwaggerConfigurationOptions
{
    [Required]
    public string TokenUrl { get; set; } = default!;

    public string ClientId { get; set; } = default!;

    public string ClientSecret { get; set; } = default!;
}