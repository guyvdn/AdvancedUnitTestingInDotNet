using WeatherService.Api.Features.AuditLogs;
using WeatherService.Core.Features.AuditLogs.Models;
using WeatherService.Testing.Unit.Core.Specifications;
using WeatherService.Testing.Unit.Features.EquivalencySteps;

namespace WeatherService.Testing.Unit.Features.MapperTests;

/// <summary>
/// <see cref="StringToAuditLogMessageEquivalencyStep"/>
/// </summary>
internal sealed class When_mapping_an_AuditLog : TestSpecificationBase
{
    [Test]
    public void It_should_map_correctly()
    {
        // Arrange
        var entity = Fixture.Create<AuditLog>();

        // Act
        var representation = entity.ToRepresentation();

        // Assert with AuditLogMessageEquivalencySteps
        representation.Should()
            .BeEquivalentTo(entity, o => o.Excluding(x => x.AuditLogId));

        entity.Should().BeEquivalentTo(representation);
    }
}