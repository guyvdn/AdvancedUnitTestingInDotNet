using System.Diagnostics;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherService.Testing.NUnit.Core.DataBase;

public static class LocalDb
{
    public static string DatabaseName { get; set; } = "UnitTests";

    public static string ConnectionString => @$"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog={DatabaseName};Integrated Security=True";

    private const string MasterConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";

    /// <summary>
    /// Create a new database (change the default name with the DatabaseName property)
    /// </summary>
    public static void Create()
    {
        var databaseFileName = Path.Combine(AppContext.BaseDirectory, DatabaseName + ".mdf");

        ExecuteCommand($"CREATE DATABASE {DatabaseName} ON (NAME = N'{DatabaseName}', FILENAME = '{databaseFileName}')",
            MasterConnectionString);
    }

    /// <summary>
    /// Drop the database (change the default name with the DatabaseName property)
    /// </summary>
    public static void Drop()
    {
        ExecuteCommand($"ALTER DATABASE [{DatabaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;" +
                       $"DROP DATABASE [{DatabaseName}]", MasterConnectionString);
    }

    /// <summary>
    /// Don't leave LocalDB process running (fix test runner warning)
    /// </summary>
    public static void Stop()
    {
        using var process = Process.Start("sqllocaldb", "stop MSSQLLocalDB");
        process?.WaitForExit();
    }

    public static void ExecuteCommand(string commandText)
    {
        ExecuteCommand(commandText, ConnectionString);
    }

    private static void ExecuteCommand(string commandText, string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        using var command = new SqlCommand(commandText, connection);
        command.ExecuteNonQuery();
    }

    /// <summary>
    /// Run FluentMigrations from the MigrationsAssembly
    /// </summary>
    public static void Migrate<TMigrationsAssembly>()
    {
        var provider = new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(runnerBuilder => runnerBuilder
                .AddSqlServer()
                .WithGlobalConnectionString(ConnectionString)
                .ScanIn(typeof(TMigrationsAssembly).Assembly).For.All())
            .AddLogging(loggingBuilder => loggingBuilder.AddFluentMigratorConsole())
            .BuildServiceProvider();

        var runner = provider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}