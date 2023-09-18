using Microsoft.Extensions.Logging;
using WeatherService.Core.Features.Images;
using WeatherService.Core.Services;
using WeatherService.Testing.Core.Fakes;
using WeatherService.Testing.Core.Specifications;

namespace WeatherService.Testing.Core.Features.Images.AddImageTests;

internal sealed class When_adding_an_image : TestSpecification<AddImage.Handler>
{
    private AddImage.Request _request;
    private CancellationToken _cancellationToken;
    private string _fileName;

    protected override void Arrange()
    {
        Fixture.Register(Build.Base64String);

        _request = Fixture.Create<AddImage.Request>();
        _fileName = Fixture.Create("FileName");
        _cancellationToken = Build.CancellationToken();

        Dependency<IFileNameService>()
            .GetImageName(_request.ConditionCode)
            .Returns(_fileName);
    }

    protected override async Task ActAsync()
    {
        await Sut.Handle(_request, _cancellationToken);
    }

    [Test]
    public void It_should_create_a_log_entry()
    {
        Dependency<FakeLogger>()
            .Messages.Should().Contain(x =>
                x.LogLevel == LogLevel.Information &&
                x.Message == $"Creating image for {_request.ConditionCode} with name {_fileName}");

        // Above is OK but this is better

        Dependency<FakeLogger>()
            .Should().LogInformation($"Creating image for {_request.ConditionCode} with name {_fileName}")
            .And.LogNoOtherMessages();
    }

    [Test]
    public void It_should_create_the_file()
    {
        var bytes = Convert.FromBase64String(_request.Base64Content);

        Dependency<IFileService>()
            .Received().SaveFileAsync(_fileName, Arg.Is<byte[]>(x => x.SequenceEqual(bytes)), _cancellationToken);

        // Above is OK but this is better

        Dependency<IFileService>()
            .Should().SaveFile(_fileName, bytes, _cancellationToken);
    }
}