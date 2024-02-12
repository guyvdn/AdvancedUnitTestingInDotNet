using Microsoft.EntityFrameworkCore;

namespace WeatherService.Testing.NUnit.Integration.Core.Infrastructure.Database;

public sealed class LocalDbContext : IDatabaseContext
{
    private readonly LocalDb _localDb;

    public LocalDbContext(string databaseName = "IntegrationTests")
    {
        _localDb = new LocalDb(databaseName);
    }

    public Task CreateAsync()
    {
        // SQL Dates are not as accurate as DateTimes
        AssertionOptions.AssertEquivalencyUsing(options =>
            options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TestPrecision.DateTimeEquals)).WhenTypeIs<DateTime>());

        _localDb.Create();

        // Run Migrations
        SqlHelper.MigrateUp<Migrations.AssemblyMarker>(_localDb.ConnectionString);
        SqlHelper.MigrateUp<AssemblyMarker>(_localDb.ConnectionString);

        return Task.CompletedTask;
    }

    public Task DeleteAsync()
    {
        _localDb.Drop();
        LocalDb.Stop();

        return Task.CompletedTask;
    }

    public void Configure(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(_localDb.ConnectionString);
    }

    public string ConnectionString => _localDb.ConnectionString;
}