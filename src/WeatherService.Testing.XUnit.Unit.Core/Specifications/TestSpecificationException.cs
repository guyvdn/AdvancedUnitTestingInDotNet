namespace WeatherService.Testing.XUnit.Unit.Core.Specifications;

[Serializable]
public class TestSpecificationException : Exception
{
    public TestSpecificationException(string? message) : base(message) { }
}