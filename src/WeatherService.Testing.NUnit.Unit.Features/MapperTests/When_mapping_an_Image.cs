using AutoFixture.NUnit3;
using WeatherService.Api.Features.Images;
using WeatherService.Representation;
using WeatherService.Testing.NUnit.Unit.Core.Specifications;

namespace WeatherService.Testing.NUnit.Unit.Features.MapperTests;

internal sealed class When_mapping_an_Image : TestSpecificationBase
{
    [Test, AutoData] // Use AutoFixture.NUnit3.AutoData to set a value
    public void It_should_map_correctly(Image image)
    {
        image.ToRequest().Should().BeEquivalentTo(image);
    }
}