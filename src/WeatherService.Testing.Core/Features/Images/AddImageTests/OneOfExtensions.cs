using OneOf;

namespace WeatherService.Testing.Core.Features.Images.AddImageTests;

public static class OneOfExtensions
{
    public static OneOfAssertions<TOneOf> Should<TOneOf>(this TOneOf oneOf) where TOneOf : struct, IOneOf
    {
        return new OneOfAssertions<TOneOf>(oneOf);
    }
}