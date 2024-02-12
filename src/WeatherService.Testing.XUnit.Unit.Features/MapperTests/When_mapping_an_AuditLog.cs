using WeatherService.Api.Features.AuditLogs;
using WeatherService.Core.Features.AuditLogs.Models;
using WeatherService.Testing.XUnit.Unit.Core.Specifications;
using WeatherService.Testing.XUnit.Unit.Core.Equivalency;

namespace WeatherService.Testing.XUnit.Unit.Features.MapperTests;

/// <summary>
/// <see cref="StringToAuditLogMessageEquivalencyStep"/>
/// </summary>
public sealed class When_mapping_an_AuditLog : TestSpecificationBase
{
    [Fact]
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