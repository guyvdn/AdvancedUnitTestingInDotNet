using System.Net.Http.Headers;
using FluentAssertions.Primitives;

namespace WeatherService.Testing.NUnit.Integration.Core.Extensions;

public static class HttpResponseHeadersExtensions
{
    public static HttpResponseHeadersAssertions Should(this HttpResponseHeaders headers)
    {
        return new HttpResponseHeadersAssertions(headers);
    }
}

public sealed class HttpResponseHeadersAssertions : ReferenceTypeAssertions<HttpResponseHeaders, HttpResponseHeadersAssertions>
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