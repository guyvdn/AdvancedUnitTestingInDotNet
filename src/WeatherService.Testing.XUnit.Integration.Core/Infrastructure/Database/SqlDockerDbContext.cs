using Microsoft.EntityFrameworkCore;

namespace WeatherService.Testing.XUnit.Integration.Core.Infrastructure.Database;

internal sealed class SqlDockerDbContext : IDatabaseContext
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
    
    public async ValueTask DisposeAsync()
    {
        await DeleteAsync();
        _sqlDockerDb.Dispose();
    }
}