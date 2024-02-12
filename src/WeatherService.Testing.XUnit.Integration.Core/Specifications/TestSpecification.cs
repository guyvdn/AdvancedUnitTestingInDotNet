using System.Net.Http.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeatherService.Testing.XUnit.Integration.Core.Extensions;
using WeatherService.Testing.XUnit.Integration.Core.Infrastructure;
using WeatherService.Testing.XUnit.Integration.Core.Infrastructure.Database;
using WeatherService.Testing.XUnit.Unit.Core.Specifications;

namespace WeatherService.Testing.XUnit.Integration.Core.Specifications;

// Generic type definition can be used for quick navigation
public abstract class TestSpecification<TController, TRequest, TResponse> : TestSpecification
    where TController : ControllerBase
    where TRequest : IBaseRequest
{
}

public abstract class TestSpecification<TController, TRequest> : TestSpecification
    where TController : ControllerBase
    where TRequest : IBaseRequest
{
}

public abstract partial class TestSpecification : TestSpecificationBase, IDisposable, IAsyncLifetime
{
    private readonly Dictionary<Type, object> _dependencies = new();

    protected TestApplicationFactory Factory { get; private set; }

    protected HttpClient Client { get; private set; }

    protected Seed Seed { get; private set; }

    protected TestSink TestSink { get; private set; }

    /// <summary>
    /// Can be used to customize AppSettings before creating the TestApplicationFactory.
    /// </summary>
    protected virtual Dictionary<string, string?> GetAppSettings()
    {
        return new Dictionary<string, string?>();
    }

    public virtual async Task InitializeAsync()
    {
        SetupDependencies();

        TestSink = new TestSink();
        Factory = new TestApplicationFactory(GetAppSettings(), _dependencies, TestSink);
        Client = Factory.CreateAuthorizedClient();
        Seed = new Seed(Factory.Services);

        await ArrangeAsync();

        ResetDependencies();

        try
        {
            await ActAsync();
        }
        catch
        {
            TestSink.PrintAllEvents();
            throw;
        }
    }

    protected async Task<TResponse> GetFromJsonAsync<TResponse>(string requestUri)
    {
        var response = await Client.GetAsync(requestUri);

        if (!response.IsSuccessStatusCode)
            Assert.Fail(
                $"""
                 Request failed with status code: {response.StatusCode}
                 reason: {response.GetProblemDetails()}
                 """);

        return await response.FromJsonAsync<TResponse>();
    }

    protected async Task<HttpResponseMessage> PostAsJsonAsync<TValue>(string uri, TValue value)
    {
        var response = await Client.PostAsJsonAsync(uri, value);

        if (!response.IsSuccessStatusCode)
            Assert.Fail(
                $"""
                 Request failed with status code: {response.StatusCode}
                 reason: {response.GetProblemDetails()}
                 """);

        return response;
    }

    protected virtual void Arrange()
    {
    }

    protected virtual Task ArrangeAsync()
    {
        Arrange();
        return Task.CompletedTask;
    }

    protected abstract Task ActAsync();

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
            return;

        HttpMessageHandler.Dispose();
        Factory.Dispose();
        Client.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
