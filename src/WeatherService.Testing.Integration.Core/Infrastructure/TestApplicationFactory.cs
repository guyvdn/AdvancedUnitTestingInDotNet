using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using WeatherService.Core.DatabaseConfiguration.DbContexts;
using WeatherService.Testing.Unit.Core;

namespace WeatherService.Testing.Integration.Core.Infrastructure;

public sealed class TestApplicationFactory : WebApplicationFactory<Api.AssemblyMarker>
{
    private readonly Dictionary<string, string?> _appSettings;
    private readonly Dictionary<Type, object> _dependencies;
    private readonly TestSink _testSink;

    public TestApplicationFactory(
        Dictionary<string, string?> appSettings,
        Dictionary<Type, object> dependencies,
        TestSink testSink)
    {
        _appSettings = appSettings;
        _dependencies = dependencies;
        _testSink = testSink;
    }

    public bool RequestIsAuthenticated { get; set; } = true;

    public string UserName { get; set; } = Build.Email();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseKestrel();

        builder.UseEnvironment(environment: "IntegrationTesting");

        builder.ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddJsonFile(path: "appsettings.json");
            config.AddJsonFile(path: $"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true);
            config.AddInMemoryCollection(_appSettings);
        });

        builder.UseSerilog((_, loggerConfiguration) =>
        {
            loggerConfiguration
                .WriteTo.Sink(_testSink)
                .MinimumLevel.Verbose()
                .MinimumLevel.Override(source: "Microsoft", LogEventLevel.Error);
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IHostedService>();
            services.RemoveAll<IConfigureOptions<AuthenticationOptions>>();

            services
                .AddAuthentication(defaultScheme: TestAuthHandler.AuthenticationScheme)
                .AddScheme<TestAuthenticationSchemeOptions, TestAuthHandler>(
                    authenticationScheme: TestAuthHandler.AuthenticationScheme,
                    o =>
                    {
                        o.RequestIsAuthenticated = RequestIsAuthenticated;
                        o.UserName = UserName;
                    });

            foreach (var mock in _dependencies)
            {
                services.AddSingleton(mock.Key, _ => mock.Value);
            }

            var dbContextServiceDescriptor = services.Single(descriptor =>
                descriptor.ServiceType == typeof(DbContextOptions<WeatherApiDbContext>)
            );
            services.Remove(dbContextServiceDescriptor);

            services.AddDbContextFactory<WeatherApiDbContext>(o =>
            {
                o.UseTestDatabaseContext(DatabaseContext.Current);
                EnableDbContextLogging(o);
            });
        });
    }

    public HttpClient CreateAuthorizedClient()
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: TestAuthHandler.AuthenticationScheme);
        return client;
    }

    private static void EnableDbContextLogging(DbContextOptionsBuilder o)
    {
#if DEBUG
        o.LogTo(TestContext.WriteLine, LogLevel.Trace);
        o.EnableSensitiveDataLogging();
#endif
    }
}