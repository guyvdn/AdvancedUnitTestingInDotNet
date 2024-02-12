using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace WeatherService.Testing.NUnit.Integration.Core.Infrastructure;

public static class JsonTextSerializer
{
    private static readonly JsonSerializerOptions _defaultOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() },
    };

    public static string Serialize(this object value)
    {
        return JsonSerializer.Serialize(value, _defaultOptions);
    }

    public static T? Deserialize<T>(this string value)
    {
        return JsonSerializer.Deserialize<T>(value, _defaultOptions);
    }

    public static T? Deserialize<T>(this BinaryData value)
    {
        return JsonSerializer.Deserialize<T>(value, _defaultOptions);
    }

    public static T? Deserialize<T>(this JsonObject value)
    {
        return JsonSerializer.Deserialize<T>(value.ToJsonString(), _defaultOptions);
    }

    public static TClass? Deserialize<TClass>(this string value, JsonConverter jsonConverter)
    {
        JsonSerializerOptions jsonSerializerOptions = new();
        jsonSerializerOptions.Converters.Add(jsonConverter);
        return JsonSerializer.Deserialize<TClass>(value, jsonSerializerOptions);
    }
}