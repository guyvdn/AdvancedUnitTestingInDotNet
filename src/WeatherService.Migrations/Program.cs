using CommandLine;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherService.Migrations;

internal sealed class Program
{
    private static void Main(string[] args)
    {
        Parser.Default.ParseArguments<MigrationOptions>(args).WithParsed(o =>
        {
            new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    .WithGlobalConnectionString(o.ConnectionString)
                    .ScanIn(typeof(AssemblyMarker).Assembly).For.All())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false)
                .GetRequiredService<IMigrationRunner>()
                .MigrateUp();
        });
    }

    private class MigrationOptions
    {
        [Option("ConnectionString", Required = true)]
        public string ConnectionString { get; set; } = default!;
    }
}