using FluentAssertions.Primitives;

namespace WeatherService.Testing.Integration.Core.Infrastructure.Logging;

public sealed class TestSinkAssertions : ReferenceTypeAssertions<TestSink, TestSinkAssertions>
{
    public TestSinkAssertions(TestSink subject)
        : base(subject)
    {
    }

    protected override string Identifier => "LogEvents";

    public AndConstraint<TestSinkAssertions> Log(string messageTemplate, string because = "")
    {
        Subject.MessageTemplateTexts().Should().Contain(messageTemplate, because);

        return new AndConstraint<TestSinkAssertions>(this);
    }

    public AndConstraint<TestSinkAssertions> LogInOrder(IEnumerable<string> messageTemplates, string because = "")
    {
        Subject.MessageTemplateTexts().Should().ContainInOrder(messageTemplates, because);

        return new AndConstraint<TestSinkAssertions>(this);
    }
}