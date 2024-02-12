using RichardSzalay.MockHttp;
using WeatherService.Testing.XUnit.Integration.Core.Infrastructure;

namespace WeatherService.Testing.XUnit.Integration.Core.Specifications;

public abstract partial class TestSpecification
{
    protected MockHttpMessageHandler HttpMessageHandler = default!;

    private void MockHttpClient()
    {
        HttpMessageHandler = new MockHttpMessageHandler();
        AddDependency<IHttpClientFactory>(new MockHttpClientFactory(HttpMessageHandler));
    }

    [Fact]
    public virtual void It_should_execute_the_expected_Http_requests()
    {
        HttpMessageHandler.VerifyNoOutstandingRequest();
        HttpMessageHandler.VerifyNoOutstandingExpectation();
    }
}