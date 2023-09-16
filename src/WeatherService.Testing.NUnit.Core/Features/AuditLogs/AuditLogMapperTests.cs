using WeatherService.Testing.NUnit.Core.Specifications;
using WeatherService.Core.Features.AuditLogs.Models;
using WeatherService.Api.Features.AuditLogs;

namespace WeatherService.Testing.NUnit.Core.Features.AuditLogs;

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