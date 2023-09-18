using WeatherService.Api.Features.AuditLogs;
using WeatherService.Core.Features.AuditLogs.Models;
using WeatherService.Testing.Unit.Core.Specifications;

namespace WeatherService.Testing.Unit.Features.Features.MapperTests;

internal sealed class When_mapping_an_AuditLog : TestSpecificationBase
{
    [Test]
    public void It_should_map_correctly()
    {
        // Arrange
        var entity = Fixture.Create<AuditLog>();

        // Act
        var representation = entity.ToRepresentation();

        // Assert
        representation.Should().BeEquivalentTo(entity, o => o.Excluding(x => x.AuditLogId));
        entity.Should().BeEquivalentTo(representation);
    }
}