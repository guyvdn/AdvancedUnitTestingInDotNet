using FluentValidation.TestHelper;
using WeatherService.Core.Features.Images;
using WeatherService.Testing.Core.Specifications;

namespace WeatherService.Testing.Core.Features.Images;

internal sealed class AddImageValidatorTests: TestSpecification<AddImage.Validator>
{
    [TestCase("")]
    [TestCase((string)null)]
    public void Should_have_error_when_Base64Content_is_null_or_empty(string? base64Content)
    {
        // Arrange
        var request = Fixture
            .Build<AddImage.Request>()
            .With(x => x.Base64Content, base64Content)
            .Create();

        // Act
        var result = Sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Base64Content)
            .WithErrorCode("NotEmptyValidator");
    }

    [Test]
    public void Should_have_error_when_Base64Content_is_not_a_valid_base64_content()
    {
        // Arrange
        const string content = "000";
        var convert = () => Convert.FromBase64String(content);
        convert.Should().Throw<Exception>();

        var request = Fixture
            .Build<AddImage.Request>()
            .With(x => x.Base64Content, content)
            .Create();

        // Act
        var result = Sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Base64Content)
            .WithErrorMessage("The provided Base64Content is not valid");
    }

    [Test]
    public void Should_not_have_error_when_Base64Content_is_a_valid_base64_content()
    {
        // Arrange
        var request = Fixture
            .Build<AddImage.Request>()
            .With(x => x.Base64Content, Build.Base64String())
            .Create();

        // Act
        var result = Sut.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Base64Content);
    }
}