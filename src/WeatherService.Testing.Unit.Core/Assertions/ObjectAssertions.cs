﻿using FluentAssertions.Primitives;

namespace WeatherService.Testing.Unit.Core.Assertions;

public static class ObjectAssertions
{
    public static void NotHaveInvocations<TSubject, TAssertions>(this ReferenceTypeAssertions<TSubject, TAssertions> objectAssertions)
        where TAssertions : ReferenceTypeAssertions<TSubject, TAssertions> where TSubject : class
    {
        objectAssertions.Subject.ReceivedCalls().Should().HaveCount(0);
    }
}