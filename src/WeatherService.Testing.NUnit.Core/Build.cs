using System.Text;
using System.Text.Json;

namespace WeatherService.Testing.NUnit.Core;

public class Build
{
    private static Random Random = new();

    public void WithSeed(int seed)
    {
        Random = new Random(seed);
    }

    public static string String(int length = 20)
    {
        const string chars = " abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        return new string(value: Enumerable
            .Repeat(element: chars, count: length)
            .Select(selector: s => s[index: Random.Next(maxValue: s.Length)])
            .ToArray());
    }

    public static string Word(int length = 20)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(value: Enumerable
            .Repeat(element: chars, count: length)
            .Select(selector: s => s[index: Random.Next(maxValue: s.Length)])
            .ToArray());
    }

    public static string WordWithPrefix(string prefix, int length = 5)
    {
        return prefix + '-' + Word(length);
    }

    public static string Email()
    {
        return $"{Word(5)}.{Word(10)}@{Word(10)}.{Word(3)}";
    }

    public static short Short(short minValue = short.MinValue, short maxValue = short.MaxValue)
    {
        return (short)Random.Next(minValue, maxValue);
    }

    public static int Int(int minValue = int.MinValue, int maxValue = int.MaxValue)
    {
        return Random.Next(minValue, maxValue);
    }

    public static long Long(long minValue = long.MinValue, long maxValue = long.MaxValue)
    {
        return Random.NextInt64(minValue, maxValue);
    }

    public static decimal Decimal()
    {
        return Convert.ToDecimal(value: Random.NextDouble());
    }

    public static byte[] Bytes()
    {
        return Encoding.ASCII.GetBytes(s: String());
    }

    public static Guid Guid()
    {
        return System.Guid.NewGuid();
    }

    public static Uri Uri()
    {
        return new Uri(uriString: "https://localhost/" + String());
    }

    public static bool Bool()
    {
        return Random.Next(minValue: 0, maxValue: 2) == 1;
    }

    public static DateTime DateTime(DateTime? start = null, DateTime? end = null)
    {
        var min = start ?? System.DateTime.UtcNow.AddYears(value: -1);
        var max = end ?? System.DateTime.UtcNow.AddYears(value: 1);
        var minutesDiff = Convert.ToInt32(value: max.Subtract(value: min).TotalMinutes + 1);
        return min.AddMinutes(value: Random.Next(minValue: 1, maxValue: minutesDiff));
    }

    public static DateTime DateInThePast(DateTime? start = null, DateTime? end = null)
    {
        var min = start ?? System.DateTime.UtcNow.AddYears(value: -1);
        var max = end ?? System.DateTime.UtcNow;
        var minutesDiff = Convert.ToInt32(value: max.Subtract(value: min).TotalMinutes + 1);
        return min.AddMinutes(value: Random.Next(minValue: 1, maxValue: minutesDiff));
    }

    public static DateTime DateInTheFuture(DateTime? start = null, DateTime? end = null)
    {
        var min = start ?? System.DateTime.UtcNow;
        var max = end ?? System.DateTime.UtcNow.AddYears(value: 1);
        var minutesDiff = Convert.ToInt32(value: max.Subtract(value: min).TotalMinutes + 1);
        return min.AddMinutes(value: Random.Next(minValue: 1, maxValue: minutesDiff));
    }

    public static DateTimeOffset DateTimeOffset(DateTimeOffset? min = default, DateTimeOffset? max = default)
    {
        min ??= System.DateTimeOffset.MinValue;
        max ??= System.DateTimeOffset.MaxValue;

        var range = max - min;

        return min.Value + new TimeSpan(ticks: Random.NextInt64(maxValue: range.Value.Ticks));
    }

    public static CancellationToken CancellationToken()
    {
        return new CancellationTokenSource().Token;
    }

    public static CancellationToken CancellationToken(TimeSpan delay)
    {
        return new CancellationTokenSource((int)delay.TotalMilliseconds).Token;
    }

    public static T EnumValue<T>()
    {
        var values = Enum.GetValues(typeof(T));
        var index = Int(minValue: 0, values.Length);
        return (T)values.GetValue(index)!;
    }

    public static string Json<T>(T value)
    {
        return JsonSerializer.Serialize(value);
    }

    public static T OneOf<T>(params T[] values)
    {
        return values[Random.Next(minValue: 0, values.Length)];
    }

    public static IEnumerable<T> Many<T>(Func<T> factory, int count = 3)
    {
        for (var i = 0; i < count; i++)
        {
            yield return factory();
        }
    }

    public static IEnumerable<T> InRandomOrder<T>(IEnumerable<T> items)
    {
        return items.OrderBy(_ => Random.Next());
    }

    public static string Base64String()
    {
        return Convert.ToBase64String(Bytes());
    }
}