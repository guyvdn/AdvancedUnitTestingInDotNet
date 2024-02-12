using Microsoft.EntityFrameworkCore;

namespace WeatherService.Testing.XUnit.Integration.Core.Infrastructure.Database;

public interface IDatabaseContext: IAsyncDisposable
{
    public Task CreateAsync();
    public Task DeleteAsync();
    public void Configure(DbContextOptionsBuilder options);
    string ConnectionString { get; }
}