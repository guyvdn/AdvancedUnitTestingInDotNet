﻿using FluentAssertions.Primitives;
using OneOf;
using OneOf.Types;

namespace WeatherService.Testing.Unit.Core.Assertions;

public static class OneOfAssertionsExtensions
{
    public static OneOfAssertions<TOneOf> Should<TOneOf>(this TOneOf oneOf) where TOneOf : struct, IOneOf
    {
        return new OneOfAssertions<TOneOf>(oneOf);
    }
}

public class OneOfAssertions<TOneOf> : ReferenceTypeAssertions<TOneOf, OneOfAssertions<TOneOf>> where TOneOf : struct, IOneOf
{
    protected override string Identifier => nameof(OneOf);

    public OneOfAssertions(TOneOf subject) : base(subject)
    {
    }

    public AndWhichConstraint<FluentAssertions.Primitives.ObjectAssertions, None> BeNone()
    {
        return Subject.Value.Should().BeOfType<None>();
    }   
    
    public AndWhichConstraint<FluentAssertions.Primitives.ObjectAssertions, NotFound> BeNotFound()
    {
        return Subject.Value.Should().BeOfType<NotFound>();
    }

    public AndWhichConstraint<FluentAssertions.Primitives.ObjectAssertions, TTo> Be<TTo>()
    {
        return Subject.Value.Should().BeOfType<TTo>();
    }

    public AndConstraint<FluentAssertions.Primitives.ObjectAssertions> Be<TTo>(TTo to)
    {
        return Subject.Value.Should().Be(to);
    }

    public AndConstraint<FluentAssertions.Primitives.ObjectAssertions> BeEquivalentTo<TTo>(TTo to)
    {
        return Subject.Value.Should().BeEquivalentTo(to);
    }
}
