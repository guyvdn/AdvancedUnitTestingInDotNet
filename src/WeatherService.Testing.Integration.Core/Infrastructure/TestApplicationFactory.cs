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
using WeatherService.Testing.NUnit.Core.DataBase;

namespace WeatherService.Testing.Integration.Core.Infrastructure;

internal sealed class TestApplicationFactory : WebApplicationFactory<Api.AssemblyMarker>
{
    private readonly Dictionary<string, string?> _appSettings;
    private readonly Dictionary<Type, object> _mocks;

    public TestApplicationFactory(Dictionary<Type, object> mocks, Dictionary<string, string?> appSettings)
    {
        _appSettings = appSettings;
        _mocks = mocks;
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
                .WriteTo.Sink<TestSink>()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override(source: "Microsoft", LogEventLevel.Warning);
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IHostedService>();
            services.RemoveAll<IConfigureOptions<AuthenticationOptions>>();

            services
                .AddAuthentication(defaultScheme: "TestScheme")
                .AddScheme<TestAuthenticationSchemeOptions, TestAuthHandler>(authenticationScheme: "TestScheme", o =>
                {
                    o.RequestIsAuthenticated = RequestIsAuthenticated;
                    o.UserName = UserName;
                });

            foreach (var mock in _mocks)
            {
                services.AddSingleton(mock.Key, _ => mock.Value);
            }

            var dbContextServiceDescriptor = services.Single(descriptor =>
                descriptor.ServiceType == typeof(DbContextOptions<WeatherApiDbContext>)
            );
            services.Remove(dbContextServiceDescriptor);

            services.AddDbContextFactory<WeatherApiDbContext>(o =>
            {
                o.UseSqlServer(LocalDb.ConnectionString);
                EnableLogging(o, false);
            });

            //services.AddDbContext<WeatherApiDbContext>(o =>
            //{
            //    o.UseSqlServer(LocalDb.ConnectionString);
            //    EnableLogging(o, false);
            //});
        });
    }

    private static void EnableLogging(DbContextOptionsBuilder o, bool enable)
    {
        if (!enable)
            return;

        o.LogTo(TestContext.WriteLine, LogLevel.Trace);
        o.EnableSensitiveDataLogging();
    }
}