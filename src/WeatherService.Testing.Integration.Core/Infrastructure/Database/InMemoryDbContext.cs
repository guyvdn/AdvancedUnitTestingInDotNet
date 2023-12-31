﻿using Microsoft.EntityFrameworkCore;

namespace WeatherService.Testing.Integration.Core.Infrastructure.Database;

internal sealed class InMemoryDbContext : IDatabaseContext
{
    public Task CreateAsync() => Task.CompletedTask;

    public Task DeleteAsync() => Task.CompletedTask;

    public void Configure(DbContextOptionsBuilder options)
    {
        options.UseInMemoryDatabase("IntegrationTests" + Guid.NewGuid());
    }

    public string ConnectionString => "";
}