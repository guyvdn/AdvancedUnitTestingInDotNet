namespace WeatherService.Testing.NUnit.Core.Specifications;

[Serializable]
public class TestSpecificationException : Exception
{
    public TestSpecificationException(string? message) : base(message) { }
}