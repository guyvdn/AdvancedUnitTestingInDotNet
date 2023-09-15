namespace WeatherService.Core.Services;

internal interface IFileNameService
{
    string GetImageName(int conditionCode);
}

internal sealed class FileNameService : IFileNameService
{
    public string GetImageName(int conditionCode)
    {
        return $"Images/{conditionCode}.png";
    }
}