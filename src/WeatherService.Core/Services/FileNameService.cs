namespace WeatherService.Core.Services;

public interface IFileNameService
{
    string GetImageName(int conditionCode);
}

public sealed class FileNameService : IFileNameService
{
    public string GetImageName(int conditionCode)
    {
        return $"Images/{conditionCode}.png";
    }
}