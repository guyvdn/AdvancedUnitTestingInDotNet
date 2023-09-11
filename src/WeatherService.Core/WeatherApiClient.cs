using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using WeatherService.Core.Features.CurrentWeather;

namespace WeatherService.Core;

public interface IWeatherApiClient
{
    Task<WeatherResponse> GetCurrentWeatherAsync(string? city, CancellationToken cancellationToken);
}

internal sealed class WeatherApiClient : IWeatherApiClient
{
    private readonly HttpClient _httpClient;
    private readonly WeatherApiOptions _options;

    public WeatherApiClient(HttpClient httpClient, IOptions<WeatherApiOptions> options)
    {
        _options = options.Value;

        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.weatherapi.com");
    }

    public async Task<WeatherResponse> GetCurrentWeatherAsync(string? city, CancellationToken cancellationToken)
    {
        var requestUri = $"v1/current.json?key={_options.ApiKey}&q=Belgium";

        var response = await _httpClient.GetFromJsonAsync<WeatherResponse>(requestUri, cancellationToken);

        return response!;
    }
}