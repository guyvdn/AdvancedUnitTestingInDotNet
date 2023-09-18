﻿using System.Net.Http.Json;
using RichardSzalay.MockHttp;
using WeatherService.Api.Features.WeatherForecast;
using WeatherService.Core.Features.WeatherForecasts;
using WeatherService.Core.Features.WeatherForecasts.Models;
using WeatherService.Representation;
using WeatherService.Testing.Integration.Core.Infrastructure;
using WeatherService.Testing.Integration.Core.Infrastructure.Logging;

namespace WeatherService.Testing.Integration.Core.Features.GetCurrentWeatherTests;

internal sealed class When_all_is_good
    : TestSpecification<WeatherForecastsController, GetCurrentWeather.Request, WeatherForecast>
{
    private WeatherForecast _response;
    private WeatherResponse _weather;

    protected override void Arrange()
    {
        _weather = Fixture.Create<WeatherResponse>();

        HttpMessageHandler
            .Expect("https://weatherapi/v1/current.json")
            .WithQueryString("key", "ApiKey")
            .WithQueryString("q", "Belgium")
            .Respond(JsonContent.Create(_weather));
    }

    protected override async Task ActAsync()
    {
        _response = await GetFromJsonAsync<WeatherForecast>("WeatherForecasts?city=Mechelen");
    }

    [Test]
    public void It_should_return_the_correct_response()
    {
        _response.Should().NotBeNull();
        _response!.TemperatureC.Should().Be(_weather.Current.TemperatureC);
    }

    [Test]
    public async Task It_should_have_created_an_audit_log()
    {
        var auditLogs = await GetFromJsonAsync<IEnumerable<AuditLog>>("AuditLogs?numberOfDays=1");

        var latestLog = auditLogs.OrderByDescending(x => x.TimeStampUTC).First();
        latestLog.TimeStampUTC.Should().BeCloseTo(DateTime.UtcNow, TestPrecision.DateTimeCloseTo);
        latestLog.Message.Should().Be("Get current weather was called for city 'Mechelen'");
        latestLog.NameIdentifier.Should().Be(Factory.UserName);
    }

    [Test]
    public void It_should_log_the_expected_messages()
    {
        TestSink.Should().Log("{User} is requesting weather for {City}");
    }
}