using FluentAssertions.Primitives;
using OneOf;
using OneOf.Types;

namespace WeatherService.Testing.Core.Features.Images.AddImageTests;

public class OneOfAssertions<TOneOf> : ReferenceTypeAssertions<TOneOf, OneOfAssertions<TOneOf>> where TOneOf : struct, IOneOf
{
    protected override string Identifier => nameof(OneOf);

    public OneOfAssertions(TOneOf subject) : base(subject)
    {
    }

    public AndWhichConstraint<ObjectAssertions, None> BeNone()
    {
        return Subject.Value.Should().BeOfType<None>();
    }   
    
    public AndWhichConstraint<ObjectAssertions, NotFound> BeNotFound()
    {
        return Subject.Value.Should().BeOfType<NotFound>();
    }

    public AndWhichConstraint<ObjectAssertions, TTo> Be<TTo>()
    {
        return Subject.Value.Should().BeOfType<TTo>();
    }

    public AndConstraint<ObjectAssertions> Be<TTo>(TTo to)
    {
        return Subject.Value.Should().Be(to);
    }

    public AndConstraint<ObjectAssertions> BeEquivalentTo<TTo>(TTo to)
    {
        return Subject.Value.Should().BeEquivalentTo(to);
    }
}