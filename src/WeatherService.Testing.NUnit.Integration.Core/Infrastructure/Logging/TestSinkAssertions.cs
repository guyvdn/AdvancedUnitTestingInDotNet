using FluentAssertions.Primitives;

namespace WeatherService.Testing.NUnit.Integration.Core.Infrastructure.Logging;

public static class TestSinkAssertionsExtensions
{
    public static TestSinkAssertions Should(this TestSink testSink)
    {
        return new TestSinkAssertions(testSink);
    }
}

public sealed class TestSinkAssertions : ReferenceTypeAssertions<TestSink, TestSinkAssertions>
{
    public TestSinkAssertions(TestSink subject)
        : base(subject)
    {
    }

    protected override string Identifier => "LogEvents";

    public AndConstraint<TestSinkAssertions> Log(string messageTemplate, string because = "")
    {
        MessageTemplateTexts().Should().Contain(messageTemplate, because);

        return new AndConstraint<TestSinkAssertions>(this);
    }

    public AndConstraint<TestSinkAssertions> LogInOrder(IEnumerable<string> messageTemplates, string because = "")
    {
        MessageTemplateTexts().Should().ContainInOrder(messageTemplates, because);

        return new AndConstraint<TestSinkAssertions>(this);
    }

    private IEnumerable<string> MessageTemplateTexts()
    {
        return Subject.Logs.Select(x => x.MessageTemplate.Text);
    }
}