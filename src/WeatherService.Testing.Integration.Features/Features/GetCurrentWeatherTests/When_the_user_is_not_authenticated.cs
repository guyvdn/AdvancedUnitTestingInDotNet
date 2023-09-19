using System.Net;
using WeatherService.Api.Features.WeatherForecasts;
using WeatherService.Core.Features.WeatherForecasts;
using WeatherService.Representation;
using WeatherService.Testing.Integration.Core.Specifications;

namespace WeatherService.Testing.Integration.Features.Features.GetCurrentWeatherTests;

internal sealed class When_the_user_is_not_authenticated
    : TestSpecification<WeatherForecastsController, GetCurrentWeather.Request, WeatherForecast>
{
    private HttpResponseMessage _response;

    protected override void Arrange()
    {
        // We can choose if the user is Authenticated or not
        Factory.RequestIsAuthenticated = false;
    }

    protected override async Task ActAsync()
    {
        _response = await Client.GetAsync("WeatherForecasts?city=Mechelen");
    }

    [Test]
    public void It_should_return_the_correct_response()
    {
        _response.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
    }
}