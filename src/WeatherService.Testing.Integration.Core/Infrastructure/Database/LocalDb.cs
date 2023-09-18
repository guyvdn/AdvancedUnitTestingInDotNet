using System.Diagnostics;

namespace WeatherService.Testing.Integration.Core.Infrastructure.Database;

public sealed class LocalDb
{
    public LocalDb(string databaseName  = "IntegrationTests")
    {
        DatabaseName = databaseName;
    }

    public string DatabaseName { get; }

    public string ConnectionString => @$"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog={DatabaseName};Integrated Security=True";

    private const string MasterConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";

    /// <summary>
    /// Create a new database (change the default name with the DatabaseName property)
    /// </summary>
    public void Create()
    {
        var databaseFileName = Path.Combine(AppContext.BaseDirectory, DatabaseName + ".mdf");

        SqlHelper.ExecuteCommand($"CREATE DATABASE {DatabaseName} ON (NAME = N'{DatabaseName}', FILENAME = '{databaseFileName}')",
            MasterConnectionString);
    }

    /// <summary>
    /// Drop the database (change the default name with the DatabaseName property)
    /// </summary>
    public void Drop()
    {
        SqlHelper.ExecuteCommand($"ALTER DATABASE [{DatabaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;" +
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
}