using System.Net;
using NSubstitute;
using WeatherService.Api.Features.Images;
using WeatherService.Core.Features.Images;
using WeatherService.Representation;
using WeatherService.Testing.Integration.Core;
using WeatherService.Testing.Integration.Core.Extensions;
using WeatherService.Testing.Integration.Core.Specifications;

namespace WeatherService.Testing.Integration.Features.Features.ImageTests;

internal sealed class When_all_is_good : TestSpecification<ImagesController, AddImage.Request>
{
    private Image _image;
    private HttpResponseMessage _response;

    protected override void Arrange()
    {
        _image = Fixture.Create<Image>();
    }

    protected override async Task ActAsync()
    {
        _response = await PostAsJsonAsync("Images", _image);
    }

    [Test]
    public async Task It_should_return_the_correct_result()
    {
        _response.Should().HaveStatusCode(HttpStatusCode.Created);
        _response.Headers.Should().HaveLocation($"http://localhost/Images?conditionCode={_image.ConditionCode}");
        await _response.Content.Should().BeEquivalentTo(_image);
    }
}