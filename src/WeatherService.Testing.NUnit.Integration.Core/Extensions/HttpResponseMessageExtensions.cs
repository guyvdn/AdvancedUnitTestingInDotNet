using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using WeatherService.Testing.NUnit.Integration.Core.Infrastructure;

namespace WeatherService.Testing.NUnit.Integration.Core.Extensions;

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
        if (message.StatusCode == HttpStatusCode.BadRequest)
        {
            var problemdetails = JsonSerializer.Deserialize<HttpValidationProblemDetails>(message.Content.ReadAsStream());
            if (problemdetails is not null) 
                return string.Join(", ", problemdetails.Errors.SelectMany(x => x.Value));
        }

        return message.ReasonPhrase ?? @"¯\_(ツ)_/¯";
    }
}