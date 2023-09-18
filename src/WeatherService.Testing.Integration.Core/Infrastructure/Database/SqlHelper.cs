using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherService.Testing.Integration.Core.Infrastructure.Database;

public static class SqlHelper
{
    /// <summary>
    /// Run FluentMigrations from the MigrationsAssembly
    /// </summary>
    public static void MigrateUp<TMigrationsAssembly>(string connectionString)
    {
        var provider = new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(runnerBuilder => runnerBuilder
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(TMigrationsAssembly).Assembly).For.All())
            .AddLogging(loggingBuilder => loggingBuilder.AddFluentMigratorConsole())
            .BuildServiceProvider();

        var runner = provider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }

    public static void ExecuteCommand(string commandText, string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        using var command = new SqlCommand(commandText, connection);
        command.ExecuteNonQuery();
    }

    public static IEnumerable<object[]> ExecuteReader(string commandText, string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        using var command = new SqlCommand(commandText, connection);
        var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var values = new object[reader.FieldCount];
            reader.GetValues(values);
            yield return values;
        }

        reader.Close();
    }
}