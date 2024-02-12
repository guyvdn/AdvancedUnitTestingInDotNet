using Microsoft.EntityFrameworkCore;

namespace WeatherService.Testing.NUnit.Integration.Core.Infrastructure.Database;

internal sealed class SqlDockerDbContext : IDatabaseContext, IDisposable
{
    private readonly SqlDockerDb _sqlDockerDb = new();

    public string ConnectionString => _sqlDockerDb.ConnectionString;

    public async Task CreateAsync()
    {
        await _sqlDockerDb.CreateAsync();
        SqlHelper.MigrateUp<Migrations.AssemblyMarker>(_sqlDockerDb.ConnectionString);
        SqlHelper.MigrateUp<AssemblyMarker>(_sqlDockerDb.ConnectionString);
    }

    public async Task DeleteAsync()
    {
        await _sqlDockerDb.DeleteAsync();
    }

    public void Configure(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(_sqlDockerDb.ConnectionString);
    }

    public void Dispose()
    {
        _sqlDockerDb.Dispose();
    }
}