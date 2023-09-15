using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherService.Testing.Integration.Core.Features.GetAuditLogsTests;

internal sealed class Seed
{
    private readonly IServiceProvider _serviceProvider;
    private readonly HttpClient _client;

    public Seed(HttpClient httpClient, IServiceProvider serviceProvider)
    {
        _client = httpClient;
        _serviceProvider = serviceProvider;
    }

    private async Task<TResult?> PostAsync<TRequest, TResult>(string route, TRequest request)
    {
        var createResponse = await _client.PostAsJsonAsync(route, request);

        await EnsureSuccessStatus(createResponse);

        return await createResponse.Content.ReadFromJsonAsync<TResult>();
    }

    private static async Task EnsureSuccessStatus(HttpResponseMessage putResponse)
    {
        if (putResponse.StatusCode == HttpStatusCode.BadRequest)
        {
            var errorContent = await putResponse.Content.ReadAsStringAsync();
            Console.WriteLine("Bad Request Content: " + errorContent);
        }

        putResponse.EnsureSuccessStatusCode();
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