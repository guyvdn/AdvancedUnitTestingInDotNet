using EfSchemaCompare;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using WeatherService.Core.Features.AuditLogs.Models;
using WeatherService.Testing.XUnit.Integration.Core;
using WeatherService.Testing.XUnit.Integration.Core.Infrastructure.Database;

namespace WeatherService.Testing.XUnit.Integration.EfCoreConfig.Specifications;

public abstract class DbContextTestSpecification<TDbContext, TMigrationsAssembly>: IAsyncLifetime
    where TDbContext : DbContext
{
    private readonly string _databaseName = "DbContextTest" + typeof(TDbContext).Name;
    private TDbContext _dbContext = default!;
    private LocalDbContext _localDbContext = default!;

    public async Task InitializeAsync()
    {
        _localDbContext = new LocalDbContext(_databaseName);
        await _localDbContext.CreateAsync();

        _dbContext = new ServiceCollection()
            .AddDbContextFactory<TDbContext>(o => o.UseTestDatabaseContext(_localDbContext))
            .BuildServiceProvider()
            .GetRequiredService<TDbContext>();
    }

    public async Task DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        await _localDbContext.DeleteAsync();
    }
    private static bool IsDbSet(System.Reflection.PropertyInfo property)
    {
        return property.PropertyType.IsGenericType &&
               property.PropertyType.GetGenericTypeDefinition().IsAssignableTo(typeof(DbSet<>));
    }

    [Fact]
    public void Count_should_work_without_errors()
    {
        ForAllDbSets(dbSet =>
        {
            var count = () => Queryable.Count(dbSet);
            count.Should().NotThrow($"{dbSet.EntityType.Name} should be configured correctly.");
        });
    }

    [Fact]
    public void Select_should_work_without_errors()
    {
        ForAllDbSets(dbSet =>
        {
            var takeOne = () => Queryable.Take(dbSet, 1);
            takeOne.Should().NotThrow($"{dbSet.EntityType.Name} should be configured correctly.");
        });
    }

    private void ForAllDbSets(Action<dynamic> action)
    {
        foreach (var dbSetProperty in _dbContext.GetType().GetProperties().Where(IsDbSet))
        {
            var dbSet = (dynamic)dbSetProperty.GetValue(_dbContext)!;
            Console.WriteLine($"Testing {dbSet.EntityType.Name}");
            action(dbSet);
        }
    }

    /// <summary>
    /// Comment code in <see cref="AuditLogConfiguration"/> to see this test in action
    /// </summary>
    [Fact]
    public void Configuration_should_match_sql_schema()
    {
        var config = new CompareEfSqlConfig();

        var errorsToIgnore = ErrorsToIgnore();
        if (!string.IsNullOrEmpty(errorsToIgnore))
            config.IgnoreTheseErrors(errorsToIgnore);

        var comparer = new CompareEfSql(config);

        var result = comparer.CompareEfWithDb(_dbContext);

#if DEBUG
        File.WriteAllText($"SchemaErrors_{_databaseName}.txt", comparer.GetAllErrors);
#endif

        result.Should().BeFalse(Environment.NewLine + comparer.GetAllErrors);
    }

    protected virtual string ErrorsToIgnore()
    {
        return "";
    }
}