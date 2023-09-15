namespace WeatherService.Testing.Integration.Core.Extensions;

internal static class NumberExtensions
{
    public static void Times(this int number, Action action)
    {
        for (var i = 0; i < number; i++)
        {
            action();
        }
    }   
    
    public static async Task TimesAsync(this int number, Func<Task> task)
    {
        for (var i = 0; i < number; i++)
        {
            await task();
        }
    }
}