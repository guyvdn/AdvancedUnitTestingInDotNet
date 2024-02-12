using OneOf;
using OneOf.Types;
using WeatherService.Core.Features.Images;
using WeatherService.Core.Services;
using WeatherService.Testing.XUnit.Unit.Core;
using WeatherService.Testing.XUnit.Unit.Core.Assertions;
using WeatherService.Testing.XUnit.Unit.Core.Dependencies;

namespace WeatherService.Testing.XUnit.Unit.Features.FeatureTests.Images.AddImageTests;

public sealed class When_getting_an_image : TestSpecification<GetImage.Handler>
{
    private GetImage.Request _request;
    private CancellationToken _cancellationToken;
    private byte[] _bytes;
    private string _fileName;
    private OneOf<string, NotFound> _result;

    // Do not use a Mocked FileNameService
    protected override object[] ExplicitDependencies => 
        new object[] { new FileNameService() };

    protected override void Arrange()
    {
        _bytes = Build.Bytes();
        _request = Fixture.Create<GetImage.Request>();
        _cancellationToken = Build.CancellationToken();

        // Use Callback to get FileName from method args
        Dependency<IFileService>()
            .GetFileAsync(Arg.Any<string>(), _cancellationToken)
            .Returns(_bytes)
            .AndDoes(info => _fileName = info.Arg<string>());
    }

    protected override async Task ActAsync()
    {
        _result = await Sut.Handle(_request, _cancellationToken);
    }

    [Fact]
    public void It_should_return_the_correct_result()
    {
        _result.Should().Be(Convert.ToBase64String(_bytes));
    }

    [Fact]
    public void It_should_create_a_log_entry()
    {
        // Because of callback we can still use the _fileName
        Dependency<FakeLogger>()
             .Should().LogInformation($"Getting image for {_request.ConditionCode} with name {_fileName}")
             .And.LogNoOtherMessages();
    }

    [Fact]
    public void It_should_get_the_file()
    {
        // When FileName is unknown we can use more generic method with Regex
        Dependency<IFileService>()
            .Should().GetImage(_cancellationToken);
    }
}