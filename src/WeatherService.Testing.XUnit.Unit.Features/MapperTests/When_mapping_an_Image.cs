using AutoFixture.Xunit2;
using WeatherService.Api.Features.Images;
using WeatherService.Representation;
using WeatherService.Testing.XUnit.Unit.Core.Specifications;

namespace WeatherService.Testing.XUnit.Unit.Features.MapperTests;

public sealed class When_mapping_an_Image : TestSpecificationBase
{
    [Theory, AutoData] // Use AutoFixture.XUnit3.AutoData to set a value
    public void It_should_map_correctly(Image image)
    {
        image.ToRequest().Should().BeEquivalentTo(image);
    }
}