namespace WeatherService.Testing.XUnit.Unit.Features;

public abstract class
    TestSpecificationBase : WeatherService.Testing.XUnit.Unit.Core.Specifications.TestSpecificationBase
{
    static TestSpecificationBase()
    {
        TestSetupFixture.Execute();
    }
}

public abstract class TestSpecification : WeatherService.Testing.XUnit.Unit.Core.Specifications.TestSpecification
{
    static TestSpecification()
    {
        TestSetupFixture.Execute();
    }
}

public abstract class TestSpecification<TSubjectUnderTest> : WeatherService.Testing.XUnit.Unit.Core.Specifications.TestSpecification<TSubjectUnderTest>
{
    static TestSpecification()
    {
        TestSetupFixture.Execute();
    }
}