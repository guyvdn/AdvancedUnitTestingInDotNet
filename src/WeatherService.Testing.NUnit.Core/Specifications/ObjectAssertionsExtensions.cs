using FluentAssertions.Primitives;

namespace WeatherService.Testing.NUnit.Core.Specifications;

public static class ObjectAssertionsExtensions
{
    public static void NotHaveInvocations<TSubject, TAssertions>(this ReferenceTypeAssertions<TSubject, TAssertions> objectAssertions) 
        where TAssertions : ReferenceTypeAssertions<TSubject, TAssertions> where TSubject : class
    {
        objectAssertions.Subject.ReceivedCalls().Should().HaveCount(0);
    }
}