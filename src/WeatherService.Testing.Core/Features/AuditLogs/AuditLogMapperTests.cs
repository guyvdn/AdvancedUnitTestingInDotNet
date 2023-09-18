using WeatherService.Api.Features.AuditLogs;
using WeatherService.Core.Features.AuditLogs.Models;
using WeatherService.Testing.Core.Specifications;

namespace WeatherService.Testing.Core.Features.AuditLogs;

internal sealed class AuditLogMapperTests: TestSpecificationBase
{
    [Test]
    public void It_should_map_correctly()
    {
        // Arrange
        var entity = Fixture.Create<AuditLog>();

        // Act
        var representation = entity.ToRepresentation();

        // Assert
        representation.Should().BeEquivalentTo(entity);
    }
}