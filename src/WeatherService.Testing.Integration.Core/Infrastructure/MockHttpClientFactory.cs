using RichardSzalay.MockHttp;

namespace WeatherService.Testing.Integration.Core.Infrastructure;

internal sealed class MockHttpClientFactory : IHttpClientFactory
{
    private readonly MockHttpMessageHandler _mockHttpMessageHandler;

    public MockHttpClientFactory(MockHttpMessageHandler mockHttpMessageHandler)
    {
        _mockHttpMessageHandler = mockHttpMessageHandler;
    }

    public HttpClient CreateClient(string name)
    {
        return _mockHttpMessageHandler.ToHttpClient();
    }
}