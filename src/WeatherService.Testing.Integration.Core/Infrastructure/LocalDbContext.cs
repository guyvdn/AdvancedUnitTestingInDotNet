using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.EntityFrameworkCore;
using WeatherService.Testing.NUnit.Core.DataBase;

namespace WeatherService.Testing.Integration.Core.Infrastructure;

internal interface IDatabaseContext
{
    public Task CreateAsync();
    public Task DeleteAsync();
    public void Configure(DbContextOptionsBuilder options);
}

internal class LocalDbContext : IDatabaseContext
{
    public Task CreateAsync()
    {
        // SQL Dates are not as accurate as DateTimes
        AssertionOptions.AssertEquivalencyUsing(options =>
            options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TestPrecision.DateTimeEquals)).WhenTypeIs<DateTime>());

        LocalDb.Create();
        LocalDb.Migrate<Migrations.AssemblyMarker>();
        LocalDb.Migrate<AssemblyMarker>();

        return Task.CompletedTask;
    }

    public Task DeleteAsync()
    {
        LocalDb.Drop();
        LocalDb.Stop();
    }

    public void Configure(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(LocalDb.ConnectionString);
    }
}

internal class InMemoryDbContext : IDatabaseContext
{
    public Task CreateAsync() => Task.CompletedTask;

    public Task DeleteAsync() => Task.CompletedTask;

    public void Configure(DbContextOptionsBuilder options)
    {
        options.UseInMemoryDatabase("IntegrationTests" + Guid.NewGuid());
    }
}

internal sealed class SqlDockerDbContext : IDatabaseContext, IDisposable
{
    private readonly SqlDockerDb _sqlDockerDb;

    public SqlDockerDbContext()
    {
        _sqlDockerDb = new SqlDockerDb();
    }

    public async Task CreateAsync()
    {
        await _sqlDockerDb.CreateAsync();
    }

    public async Task DeleteAsync()
    {
        await _sqlDockerDb.DeleteAsync();
    }

    public void Configure(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(_sqlDockerDb.ConnectionString);
    }

    public void Dispose()
    {
        _sqlDockerDb.Dispose();
    }
}

internal sealed class SqlDockerDb: IDisposable
{
    // The password must be at least 8 characters long and contain characters from three of the following four sets: Uppercase letters, Lowercase letters, Base 10 digits, and Symbols.
    private const string Password = "8i@bEjk&AAsk";
    public static string DatabaseName { get; set; } = "UnitTests";

    public static string ConnectionString => @$"Server=localhost,1433;Database={DatabaseName};User=sa;Password={Password}";
    private const string MasterConnectionString = $"Server=localhost,1433;Database=master;User=sa;Password={Password}";
    
    private string _containerId = default!;
    private DockerClient _dockerClient = default!;

    public async Task CreateAsync()
    {
        _dockerClient = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();

        var imageParameters = new ImagesCreateParameters
        {
            FromImage = "mcr.microsoft.com/mssql/server",
            Tag = "2022-latest"
        };

        await _dockerClient.Images.CreateImageAsync(imageParameters, null, new Progress<JSONMessage>());

        var createResponse = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
        {
            Image = "mcr.microsoft.com/mssql/server:2022-latest",
            Name = "sql-server-test-container",
            Env = new List<string>
            {
                "ACCEPT_EULA=Y",
                $"SA_PASSWORD={Password}"
            },
            HostConfig = new HostConfig
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>
                {
                    { "1433/tcp", new List<PortBinding> { new PortBinding { HostPort = "1433" } } }
                }
            }
        });

        _containerId = createResponse.ID;
        await _dockerClient.Containers.StartContainerAsync(_containerId, null);
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