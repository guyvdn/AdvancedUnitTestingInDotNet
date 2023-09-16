﻿using FluentAssertions.Primitives;
using Microsoft.Extensions.Logging;

namespace WeatherService.Testing.Core.Fakes;

public sealed class FakeLoggerAssertions : ReferenceTypeAssertions<FakeLogger, FakeLoggerAssertions>
{
    private int _logCount;

    protected override string Identifier => nameof(FakeLogger);

    public FakeLoggerAssertions(FakeLogger subject) : base(subject)
    {
    }

    private AndConstraint<FakeLoggerAssertions> Log(LogLevel logLevel, string message)
    {
        Subject.Messages.Should().Contain(x => x.LogLevel == logLevel && x.Message == message);

        _logCount++;

        return new AndConstraint<FakeLoggerAssertions>(this);
    }

    public AndConstraint<FakeLoggerAssertions> LogInformation(string message)
    {
        return Log(LogLevel.Information, message);
    }   
    
    public AndConstraint<FakeLoggerAssertions> LogError(string message)
    {
        return Log(LogLevel.Error, message);
    }

    public void LogNoOtherMessages()
    {
        Subject.Messages.Should().HaveCount(_logCount);
    }
}

public static class FakeLoggerAssertionsExtensions
{
    public static FakeLoggerAssertions Should(this FakeLogger fakeLogger)
    {
        return new FakeLoggerAssertions(fakeLogger);
    }
}