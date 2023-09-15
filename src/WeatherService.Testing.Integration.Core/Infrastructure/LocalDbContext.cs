using WeatherService.Testing.NUnit.Core.DataBase;

namespace WeatherService.Testing.Integration.Core.Infrastructure;

internal static class LocalDbContext
{
    public static void Create()
    {
        // SQL Dates are not as accurate as DateTimes
        AssertionOptions.AssertEquivalencyUsing(options =>
            options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TestPrecision.DateTimeEquals)).WhenTypeIs<DateTime>());

        LocalDb.Create();
        LocalDb.Migrate<Migrations.AssemblyMarker>();
        LocalDb.Migrate<AssemblyMarker>();
    }

    public static void Cleanup()
    {
        LocalDb.Drop();
        LocalDb.Stop();
    }
}