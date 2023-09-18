using OneOf;
using OneOf.Types;
using WeatherService.Core.Features.Images;
using WeatherService.Core.Services;
using WeatherService.Testing.Core;
using WeatherService.Testing.Core.Assertions;
using WeatherService.Testing.Core.Fakes;
using WeatherService.Testing.Core.Features.Images.AddImageTests;
using WeatherService.Testing.Core.Specifications;

namespace WeatherService.Testing.Features.Features.Images.AddImageTests;

internal sealed class When_getting_an_image_that_does_not_exist : TestSpecification<GetImage.Handler>
{
    private GetImage.Request _request;
    private CancellationToken _cancellationToken;
    private string _fileName;
    private OneOf<string, NotFound> _result;

    protected override object[] ExplicitDependencies => new object[] { new FileNameService() };

    protected override void Arrange()
    {
        _request = Fixture.Create<GetImage.Request>();
        _cancellationToken = Build.CancellationToken();

        Dependency<IFileService>()
            .GetFileAsync(Arg.Any<string>(), _cancellationToken)
            .Returns(new NotFound())
            .AndDoes(info => _fileName = info.Arg<string>());
    }

    protected override async Task ActAsync()
    {
        _result = await Sut.Handle(_request, _cancellationToken);
    }

    [Test]
    public void It_should_return_the_correct_result()
    {
        _result.Should().BeNotFound();
    }

    [Test]
    public void It_should_create_a_log_entry()
    {
        Dependency<FakeLogger>()
            .Should().LogInformation($"Getting image for {_request.ConditionCode} with name {_fileName}")
            .And.LogNoOtherMessages();
    }

    [Test]
    public void It_should_get_the_file()
    {
        Dependency<IFileService>()
            .Should().GetImage(_cancellationToken);
    }
}