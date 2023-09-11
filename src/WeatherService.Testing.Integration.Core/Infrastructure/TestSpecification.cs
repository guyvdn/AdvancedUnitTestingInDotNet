using System.Net.Http.Headers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeatherService.Testing.NUnit.Core.Specifications;

namespace WeatherService.Testing.Integration.Core.Infrastructure;

internal abstract class TestSpecification<TController, TRequest> : TestSpecificationBase, IDisposable
    where TController : ControllerBase
    where TRequest : IBaseRequest
{
    private readonly Dictionary<Type, object> _mocks = new();

    protected TestApplicationFactory Factory { get; private set; } = default!;

    protected HttpClient Client { get; private set; } = default!;

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
        
        //SetupMocks();

        Client = Factory.CreateClient();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "TestScheme");
        //Seed = new Seed(Fixture, Client, Factory.Services);

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