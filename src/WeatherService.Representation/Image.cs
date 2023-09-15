namespace WeatherService.Representation;

[Serializable]
public sealed class Image
{
    public required int ConditionCode { get; set; }

    public required string Base64Content { get; set; }
}