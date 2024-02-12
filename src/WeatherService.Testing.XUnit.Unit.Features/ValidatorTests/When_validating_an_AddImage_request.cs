using FluentValidation.TestHelper;
using WeatherService.Core.Features.Images;

namespace WeatherService.Testing.XUnit.Unit.Features.ValidatorTests;

public sealed class When_validating_an_AddImage_request : TestSpecification<AddImage.Validator>
{
    [Fact]
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

        // Assert custom rule with Message
        result.ShouldHaveValidationErrorFor(x => x.Base64Content)
            .WithErrorMessage("The provided Base64Content is not valid");
    }
} 