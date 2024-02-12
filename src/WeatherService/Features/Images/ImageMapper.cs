using WeatherService.Core.Features.Images;
using WeatherService.Representation;

namespace WeatherService.Api.Features.Images;

public static class ImageMapper
{
    public static AddImage.Request ToRequest(this Image image)
    {
        return new AddImage.Request
        {
            ConditionCode = image.ConditionCode,
            Base64Content = image.Base64Content,
        };
    }
}