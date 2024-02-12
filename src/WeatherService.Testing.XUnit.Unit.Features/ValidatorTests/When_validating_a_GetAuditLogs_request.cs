using FluentValidation.TestHelper;
using WeatherService.Core.Features.AuditLogs;

namespace WeatherService.Testing.XUnit.Unit.Features.ValidatorTests;

// Validators can be tested with FluentValidation.TestHelper
public sealed class When_validating_a_GetAuditLogs_request : TestSpecification<GetAuditLogs.Validator>
{
    [Theory]
    [InlineData(0)]
    [InlineData(-123)]
    [InlineData(int.MinValue)]
    public void Should_have_error_when_NumberOfDays_is_0_or_less(int numberOfDays)
    {
        // Arrange
        var request = Fixture
            .Build<GetAuditLogs.Request>()
            .With(x => x.NumberOfDays, numberOfDays)
            .Create();

        // Act
        var result = Sut.TestValidate(request);

        // Assert
        result
            .ShouldHaveValidationErrorFor(x => x.NumberOfDays)
            .WithErrorCode("GreaterThanValidator");
    }
}