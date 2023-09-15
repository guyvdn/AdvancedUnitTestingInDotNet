namespace WeatherService.Representation;

[Serializable]
public class AuditLog
{
    public required string NameIdentifier { get; set; }

    public required string Message { get; set; }

    public required DateTime TimeStampUTC { get; set; }
}