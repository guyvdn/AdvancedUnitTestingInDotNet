using EfSchemaCompare;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WeatherService.Core.Features.AuditLogs.Models;
using WeatherService.Testing.Integration.Core;
using WeatherService.Testing.Integration.Core.Infrastructure.Database;

namespace WeatherService.Testing.EfCoreConfig.Specifications;

[TestFixture]
internal abstract class DbContextTestSpecification<TDbContext, TMigrationsAssembly>
    where TDbContext : DbContext
{
    private readonly string _databaseName = "DbContextTest" + typeof(TDbContext).Name;
    private TDbContext _dbContext = default!;
    private LocalDbContext _localDbContext = default!;

    [OneTimeSetUp]
    protected async Task OneTimeSetUp()
    {
        _localDbContext = new LocalDbContext(_databaseName);
        await _localDbContext.CreateAsync();

        _dbContext = new ServiceCollection()
            .AddDbContextFactory<TDbContext>(o => o.UseTestDatabaseContext(_localDbContext))
            .BuildServiceProvider()
            .GetRequiredService<TDbContext>();
    }

    [OneTimeTearDown]
    protected async Task OneTimeTearDown()
    {
        await _dbContext.DisposeAsync();
        await _localDbContext.DeleteAsync();
    }

    private static bool IsDbSet(System.Reflection.PropertyInfo property)
    {
        return property.PropertyType.IsGenericType &&
               property.PropertyType.GetGenericTypeDefinition().IsAssignableTo(typeof(DbSet<>));
    }

    [Test]
    public void Count_should_work_without_errors()
    {
        ForAllDbSets(dbSet =>
        {
            var count = () => Queryable.Count(dbSet);
            count.Should().NotThrow($"{dbSet.EntityType.Name} should be configured correctly.");
        });
    }

    [Test]
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
            TestContext.WriteLine($"Testing {dbSet.EntityType.Name}");
            action(dbSet);
        }
    }

    /// <summary>
    /// Comment code in <see cref="AuditLogConfiguration"/> to see this test in action
    /// </summary>
    [Test]
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