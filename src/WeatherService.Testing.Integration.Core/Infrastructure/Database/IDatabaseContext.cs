using Microsoft.EntityFrameworkCore;

namespace WeatherService.Testing.Integration.Core.Infrastructure.Database;

public interface IDatabaseContext
{
    public Task CreateAsync();
    public Task DeleteAsync();
    public void Configure(DbContextOptionsBuilder options);
    string ConnectionString { get; }
}