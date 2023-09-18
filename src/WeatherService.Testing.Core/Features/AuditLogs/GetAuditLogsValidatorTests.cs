using FluentValidation.TestHelper;
using WeatherService.Core.Features.AuditLogs;
using WeatherService.Testing.Core.Specifications;

namespace WeatherService.Testing.Core.Features.AuditLogs;

internal sealed class GetAuditLogsValidatorTests : TestSpecification<GetAuditLogs.Validator>
{
    [TestCase(1)]
    [TestCase(4)]
    [TestCase(7)]
    public void Should_not_have_error_when_NumberOfDays_is_between_1_and_7(int numberOfDays)
    {
        // Arrange
        var request = Fixture
            .Build<GetAuditLogs.Request>()
            .With(x => x.NumberOfDays, numberOfDays)
            .Create();

        // Act
        var result = Sut.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.NumberOfDays);
    }

    [TestCase(0)]
    [TestCase(-123)]
    [TestCase(int.MinValue)]
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
        result.ShouldHaveValidationErrorFor(x => x.NumberOfDays)
            .WithErrorCode("GreaterThanValidator");
    }    
    
    [TestCase(8)]
    [TestCase(123)]
    [TestCase(int.MaxValue)]
    public void Should_have_error_when_NumberOfDays_is_more_than_7(int numberOfDays)
    {
        // Arrange
        var request = Fixture
            .Build<GetAuditLogs.Request>()
            .With(x => x.NumberOfDays, numberOfDays)
            .Create();

        // Act
        var result = Sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NumberOfDays)
            .WithErrorCode("LessThanOrEqualValidator");
    }
}