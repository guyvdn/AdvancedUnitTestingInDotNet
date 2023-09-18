using AutoFixture.NUnit3;
using WeatherService.Api.Features.Images;
using WeatherService.Representation;
using WeatherService.Testing.Unit.Core.Specifications;

namespace WeatherService.Testing.Unit.Features.Features.MapperTests;

internal sealed class When_mapping_an_Image : TestSpecificationBase
{
    [Test, AutoData]
    public void It_should_map_correctly(Image image)
    {
        image.ToRequest().Should().BeEquivalentTo(image);
    }
}