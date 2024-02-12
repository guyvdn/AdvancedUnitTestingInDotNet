using FluentValidation.TestHelper;
using WeatherService.Core.Features.AuditLogs;
using WeatherService.Testing.NUnit.Unit.Core.Specifications;

namespace WeatherService.Testing.NUnit.Unit.Features.ValidatorTests;

// Validators can be tested with FluentValidation.TestHelper
internal sealed class When_validating_a_GetAuditLogs_request : TestSpecification<GetAuditLogs.Validator>
{
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
        result
            .ShouldHaveValidationErrorFor(x => x.NumberOfDays)
            .WithErrorCode("GreaterThanValidator");
    }
}