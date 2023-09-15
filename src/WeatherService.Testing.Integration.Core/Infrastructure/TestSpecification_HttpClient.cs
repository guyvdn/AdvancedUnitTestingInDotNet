using RichardSzalay.MockHttp;

namespace WeatherService.Testing.Integration.Core.Infrastructure;

internal abstract partial class TestSpecification
{
    protected MockHttpMessageHandler HttpMessageHandler = default!;

    private void MockHttpClient()
    {
        HttpMessageHandler = new MockHttpMessageHandler();
        AddMock<IHttpClientFactory>(new MockHttpClientFactory(HttpMessageHandler));
    }

    [Test]
    public virtual void It_should_execute_the_expected_Http_requests()
    {
        HttpMessageHandler.VerifyNoOutstandingRequest();
        HttpMessageHandler.VerifyNoOutstandingExpectation();
    }
}