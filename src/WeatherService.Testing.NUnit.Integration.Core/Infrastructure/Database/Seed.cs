using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherService.Testing.NUnit.Integration.Core.Infrastructure.Database;

public sealed class Seed
{
    private readonly IServiceProvider _serviceProvider;

    public Seed(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task WithDbContextAsync<TDbContext>(Func<TDbContext, Task> action)
        where TDbContext : DbContext
    {
        var dbContextFactory = _serviceProvider.GetRequiredService<IDbContextFactory<TDbContext>>();
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        await action(dbContext);
        await dbContext.SaveChangesAsync();
    }

    public void WithDbContext<TDbContext>(Action<TDbContext> action)
        where TDbContext : DbContext
    {
        var dbContext = _serviceProvider.GetRequiredService<TDbContext>();
        action(dbContext);
        dbContext.SaveChanges();
    }
}