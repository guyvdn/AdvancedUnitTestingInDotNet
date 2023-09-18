using WeatherService.Api.Features.Images;
using WeatherService.Representation;
using WeatherService.Testing.Core.Specifications;

namespace WeatherService.Testing.Features.Features.Images;

internal sealed class When_mapping_an_Image: TestSpecificationBase
{
    [Test]
    public void It_should_map_correctly()
    {
        // Arrange
        var image = Fixture.Create<Image>();

        // Act
        var request = image.ToRequest();

        // Assert
        request.Should().BeEquivalentTo(image);
    }
}