using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace WeatherService.Testing.Integration.Core.Infrastructure;

internal static class HttpResponseMessageExtensions
{
    public static async Task<T> FromJsonAsync<T>(this HttpResponseMessage message)
    {
        var responseContent = await message.Content.ReadAsStringAsync();
        var response = responseContent.Deserialize<T>();
        response.Should().NotBeNull();
        return response!;
    }

    public static string GetProblemDetails(this HttpResponseMessage message)
    {
        if (message.StatusCode != HttpStatusCode.BadRequest)
            return message.ReasonPhrase ?? @"¯\_(ツ)_/¯";

        var problemdetails = JsonSerializer.Deserialize<HttpValidationProblemDetails>(message.Content.ReadAsStream());
        if (problemdetails is null)
            return @"¯\_(ツ)_/¯";

        return string.Join(", ", problemdetails.Errors.SelectMany(x => x.Value));
    }
}