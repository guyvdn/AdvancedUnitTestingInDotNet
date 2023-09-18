using System.Net.Http.Headers;
using FluentAssertions.Primitives;

namespace WeatherService.Testing.Integration.Core.Features.ImageTests;

internal static class HttpResponseHeadersExtensions
{
    public static HttpResponseHeadersAssertions Should(this HttpResponseHeaders headers)
    {
        return new HttpResponseHeadersAssertions(headers);
    }
}

internal sealed class HttpResponseHeadersAssertions : ReferenceTypeAssertions<HttpResponseHeaders, HttpResponseHeadersAssertions>
{
    public HttpResponseHeadersAssertions(HttpResponseHeaders subject)
        : base(subject)
    {
    }

    protected override string Identifier => nameof(HttpResponseHeaders);

    public AndConstraint<HttpResponseHeadersAssertions> HaveLocation(string absoluteUri)
    {
        Subject.Location.Should().NotBeNull();
        Subject.Location!.AbsoluteUri.Should().Be(absoluteUri);

        return new AndConstraint<HttpResponseHeadersAssertions>(this);
    }
}