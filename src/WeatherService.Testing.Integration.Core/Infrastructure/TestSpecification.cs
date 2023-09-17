using System.Net.Http.Headers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using WeatherService.Testing.Integration.Core.Features.GetAuditLogsTests;
using WeatherService.Testing.Integration.Core.Features.GetCurrentWeatherTests;
using WeatherService.Testing.NUnit.Core.Specifications;

namespace WeatherService.Testing.Integration.Core.Infrastructure;

internal abstract partial class TestSpecification<TController, TRequest, TResponse>: TestSpecification
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
    private readonly Dictionary<Type, object> _mocks = new();

    protected TestApplicationFactory Factory { get; private set; } = default!;

    protected HttpClient Client { get; private set; } = default!;

    protected Seed Seed { get; private set; } = default!;

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
        Factory = new TestApplicationFactory(_mocks, GetAppSettings());

        SetupMocks();

        Client = Factory.CreateClient();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "TestScheme");
        Seed = new Seed(Client, Factory.Services);

        await ArrangeAsync();

        //ResetMocks();

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

    private void SetupMocks()
    {
        MockHttpClient();
    }

    private T AddMock<T>()
        where T : class
    {
        var mock = Substitute.For<T>();
        AddMock<T>(mock);
        return mock;
    }

    private void AddMock<T>(T mock)
    {
        _mocks.Add(typeof(T), mock!);
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