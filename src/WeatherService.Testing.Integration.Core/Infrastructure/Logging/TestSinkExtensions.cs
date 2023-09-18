namespace WeatherService.Testing.Integration.Core.Infrastructure.Logging;

public static class TestSinkExtensions
{
    public static TestSinkAssertions Should(this TestSink testSink)
    {
        return new TestSinkAssertions(testSink);
    }

    public static IEnumerable<string> MessageTemplateTexts(this TestSink testSink)
    {
        return testSink.Logs.Select(x => x.MessageTemplate.Text);
    }
}