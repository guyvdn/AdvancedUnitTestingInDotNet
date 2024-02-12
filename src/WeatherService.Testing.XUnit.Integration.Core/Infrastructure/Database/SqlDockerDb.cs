using Docker.DotNet;
using Docker.DotNet.Models;
using FluentAssertions.Extensions;
using Microsoft.Data.SqlClient;

namespace WeatherService.Testing.XUnit.Integration.Core.Infrastructure.Database;

internal sealed class SqlDockerDb : IDisposable
{
    // The password must be at least 8 characters long and contain characters from three of the following four sets: Uppercase letters, Lowercase letters, Base 10 digits, and Symbols.
    private const string Password = "8i@bEjk&AAsk";

    public string DatabaseName { get; }

    public string ConnectionString => $"Server=localhost,1433;Database={DatabaseName};User=sa;Password={Password};TrustServerCertificate=true";
    private const string MasterConnectionString = $"Server=localhost,1433;Database=master;User=sa;Password={Password};TrustServerCertificate=true";

    private string _containerId = default!;
    private DockerClient _dockerClient = default!;

    public SqlDockerDb(string databaseName = "IntegrationTests")
    {
        DatabaseName = databaseName;
    }

    public async Task CreateAsync()
    {
        _dockerClient = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();

        // await GetSqlServerImage();

        var createResponse = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
        {
            Image = "mcr.microsoft.com/mssql/server:2022-latest",
            Name = $"sql-server-test-container-{Guid.NewGuid()}",
            Env = new List<string>
            {
                "ACCEPT_EULA=Y",
                $"SA_PASSWORD={Password}"
            },
            HostConfig = new HostConfig
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>
                {
                    { "1433/tcp", new List<PortBinding> { new() { HostPort = "1433" } } }
                }
            }
        });

        _containerId = createResponse.ID;
        await _dockerClient.Containers.StartContainerAsync(_containerId, null);

        await WaitForSqlToBeStarted();

        await CreateDatabaseAsync();
    }

    private static async Task WaitForSqlToBeStarted()
    {
        var cancellationToken = Build.CancellationToken(60.Seconds());

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await using var connection = new SqlConnection(MasterConnectionString);
                await connection.OpenAsync(cancellationToken);
                return;
            }
            catch
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Delay(200, cancellationToken);
            }
        }
    }

    private Task CreateDatabaseAsync()
    {
        SqlHelper.ExecuteCommand($"CREATE DATABASE {DatabaseName}", MasterConnectionString);

        return Task.CompletedTask;
    }

    private async Task GetSqlServerImage()
    {
        var imageParameters = new ImagesCreateParameters
        {
            FromImage = "mcr.microsoft.com/mssql/server",
            Tag = "2022-latest"
        };

        await _dockerClient.Images.CreateImageAsync(imageParameters, null, new Progress<JSONMessage>());
    }

    public async Task DeleteAsync()
    {
        await _dockerClient.Containers.StopContainerAsync(_containerId, new ContainerStopParameters());
        await _dockerClient.Containers.RemoveContainerAsync(_containerId, new ContainerRemoveParameters());
    }

    public void Dispose()
    {
        _dockerClient.Dispose();
    }
}