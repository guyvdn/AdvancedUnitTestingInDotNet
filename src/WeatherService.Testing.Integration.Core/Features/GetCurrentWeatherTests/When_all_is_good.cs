using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using RichardSzalay.MockHttp;
using WeatherService.Api.Features.WeatherForecast;
using WeatherService.Core.Features.WeatherForecasts;
using WeatherService.Core.Features.WeatherForecasts.Models;
using WeatherService.Representation;
using WeatherService.Testing.Integration.Core.Infrastructure;

namespace WeatherService.Testing.Integration.Core.Features.GetCurrentWeatherTests;

internal sealed class When_all_is_good : TestSpecification<WeatherForecastsController, GetCurrentWeather.Request, WeatherForecast>
{
    private WeatherForecast? _response;

    protected override void Arrange()
    {
        var response = Fixture.Build<WeatherResponse>();

        HttpMessageHandler
            .Expect("https://weatherapi/v1/current.json")
            .WithQueryString("key", "ApiKey")
            .WithQueryString("q", "Belgium")
            .Respond(JsonContent.Create(response));
    }

    protected override async Task ActAsync()
    {
        _response = await GetFromJsonAsync<WeatherForecast>("WeatherForecasts?city=Mechelen");
    }

    [Test]
    public void It_should_return_the_correct_response()
    {
        _response.Should().NotBeNull();
    }
}


internal static class HttpResponseMessageExtensions
{
    public static async Task<T> FromJsonAsync<T>(this HttpResponseMessage message)
    {
        var responseContent = await message.Content.ReadAsStringAsync();
        var response = responseContent.Deserialize<T>();
        response.Should().NotBeNull();
        return response!;
    }

    public static string GetProblemDetails(this HttpResponseMessage message)
    {
        if (message.StatusCode != HttpStatusCode.BadRequest)
            return message.ReasonPhrase ?? @"¯\_(ツ)_/¯";

        var problemdetails = JsonSerializer.Deserialize<HttpValidationProblemDetails>(message.Content.ReadAsStream());
        if (problemdetails is null)
            return @"¯\_(ツ)_/¯";

        return string.Join(", ", problemdetails.Errors.SelectMany(x => x.Value));
    }
}