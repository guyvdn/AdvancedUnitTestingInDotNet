using OneOf;
using OneOf.Types;
using WeatherService.Core.Features.Images;
using WeatherService.Core.Services;
using WeatherService.Testing.NUnit.Unit.Core;
using WeatherService.Testing.NUnit.Unit.Core.Assertions;
using WeatherService.Testing.NUnit.Unit.Core.Specifications;

namespace WeatherService.Testing.NUnit.Unit.Features.FeatureTests.Images.AddImageTests;

internal sealed class When_getting_an_image_that_does_not_exist : TestSpecification<GetImage.Handler>
{
    private GetImage.Request _request;
    private CancellationToken _cancellationToken;
    private OneOf<string, NotFound> _result;

    protected override object[] ExplicitDependencies => new object[] { new FileNameService() };

    protected override void Arrange()
    {
        _request = Fixture.Create<GetImage.Request>();
        _cancellationToken = Build.CancellationToken();

        Dependency<IFileService>()
            .GetFileAsync(Arg.Any<string>(), _cancellationToken)
            .Returns(new NotFound());
    }

    protected override async Task ActAsync()
    {
        _result = await Sut.Handle(_request, _cancellationToken);
    }

    [Test]
    public void It_should_return_the_correct_result()
    {
        // Result is of Type OneOf<string, NotFound>

        _result.Value.Should().BeOfType<NotFound>();

        // Above is OK but this is better

        _result.Should().BeNotFound();
    }
}