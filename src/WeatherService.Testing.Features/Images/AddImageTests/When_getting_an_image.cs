using OneOf;
using OneOf.Types;
using WeatherService.Core.Features.Images;
using WeatherService.Core.Services;
using WeatherService.Testing.Core.Assertions;
using WeatherService.Testing.Core.Fakes;
using WeatherService.Testing.Core.Specifications;

namespace WeatherService.Testing.Core.Features.Images.AddImageTests;

internal sealed class When_getting_an_image : TestSpecification<GetImage.Handler>
{
    private GetImage.Request _request;
    private CancellationToken _cancellationToken;
    private byte[] _bytes;
    private string _fileName;
    private OneOf<string, NotFound> _result;

    protected override object[] ExplicitDependencies => new object[] { new FileNameService() };

    protected override void Arrange()
    {
        _bytes = Build.Bytes();
        _request = Fixture.Create<GetImage.Request>();
        _cancellationToken = Build.CancellationToken();

        Dependency<IFileService>()
            .GetFileAsync(Arg.Any<string>(), _cancellationToken)
            .Returns(_bytes)
            .AndDoes(info => _fileName = info.Arg<string>());
    }

    protected override async Task ActAsync()
    {
        _result = await Sut.Handle(_request, _cancellationToken);
    }

    [Test]
    public void It_should_return_the_correct_result()
    {
        _result.Should().Be(Convert.ToBase64String(_bytes));
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