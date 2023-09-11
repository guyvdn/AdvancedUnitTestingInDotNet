namespace WeatherService.Testing.NUnit.Core.Mocking;

public interface IMockScope
{
    TSubjectUnderTest CreateSut<TSubjectUnderTest>();
}