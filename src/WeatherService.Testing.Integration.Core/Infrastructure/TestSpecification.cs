using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeatherService.Testing.Core.Specifications;
using WeatherService.Testing.Integration.Core.Features.GetAuditLogsTests;
using WeatherService.Testing.Integration.Core.Infrastructure.Logging;

namespace WeatherService.Testing.Integration.Core.Infrastructure;

internal abstract partial class TestSpecification<TController, TRequest, TResponse> : TestSpecification
    where TController : ControllerBase
    where TRequest : IBaseRequest
{
}

internal abstract partial class TestSpecification<TController, TRequest> : TestSpecification
    where TController : ControllerBase
    where TRequest : IBaseRequest
{
}

internal abstract partial class TestSpecification : TestSpecificationBase, IDisposable
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

    [OneTimeSetUp]
    public virtual async Task OneTimeSetUp()
    {
        SetupDependencies();

        TestSink = new TestSink();
        Factory = new TestApplicationFactory(GetAppSettings(), _dependencies, TestSink);
        Client = Factory.CreateAuthorizedClient();
        Seed = new Seed(Client, Factory.Services);

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


    protected virtual void Arrange()
    {
    }

    protected virtual Task ArrangeAsync()
    {
        Arrange();
        return Task.CompletedTask;
    }

    protected abstract Task ActAsync();

    public void Dispose()
    {
        Factory.Dispose();
        Client.Dispose();
    }
}
