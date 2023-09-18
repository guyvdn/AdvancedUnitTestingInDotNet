using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions.Primitives;
using Microsoft.AspNetCore.Http;
using WeatherService.Representation;

namespace WeatherService.Testing.Integration.Core;

public static class HttpContentExtensions
{
    public static HttpContentAssertions Should(this HttpContent content)
    {
        return new HttpContentAssertions(content);
    }
}

public sealed class HttpContentAssertions : ReferenceTypeAssertions<HttpContent, HttpContentAssertions>
{
    private HttpValidationProblemDetails _problemDetails;

    private int _errorCount;

    public HttpContentAssertions(HttpContent subject)
        : base(subject)
    {
    }

    protected override string Identifier => "HttpContent";

    public AndConstraint<HttpContentAssertions> HaveProblemDetails(string? because = default)
    {
        var result = JsonSerializer.Deserialize<HttpValidationProblemDetails>(Subject.ReadAsStream());

        result.Should().NotBeNull(because);

        _problemDetails = result!;

        return new AndConstraint<HttpContentAssertions>(this);
    }

    public AndConstraint<HttpContentAssertions> ContainError(string key)
    {
        _problemDetails.Errors.Should().Contain(x => x.Key == key);

        _errorCount++;

        return new AndConstraint<HttpContentAssertions>(this);
    }

    public AndConstraint<HttpContentAssertions> ContainError(string key, string value)
    {
        _problemDetails.Errors.Should().Contain(x => x.Key == key && x.Value.Single() == value);

        _errorCount++;

        return new AndConstraint<HttpContentAssertions>(this);
    }

    public AndConstraint<HttpContentAssertions> ContainNoOtherErrors()
    {
        _errorCount.Should().Be(_problemDetails.Errors.Count, string.Join(separator: ", ", _problemDetails.Errors.Keys));

        return new AndConstraint<HttpContentAssertions>(this);
    }

    public AndConstraint<HttpContentAssertions> ContainNoErrors()
    {
        var result = JsonSerializer.Deserialize<HttpValidationProblemDetails>(Subject.ReadAsStream());

        result.Should().BeNull(string.Join(separator: ", ", result!.Errors.Keys));

        return new AndConstraint<HttpContentAssertions>(this);
    }

    public async Task BeEquivalentTo<TExpectation>(TExpectation expectation)
    {
        var content = await Subject.ReadFromJsonAsync<Image>();
        content.Should().BeEquivalentTo(expectation);
    }
}