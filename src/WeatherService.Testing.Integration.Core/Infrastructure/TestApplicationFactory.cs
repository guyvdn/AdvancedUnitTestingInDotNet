using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;

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

    //protected override IWebHostBuilder CreateWebHostBuilder()
    //{
    //    return new WebHostBuilder()
    //        .UseKestrel()
    //        .UseContentRoot(Directory.GetCurrentDirectory())
    //        .ConfigureAppConfiguration((hostingContext, config) =>
    //        {
    //            config.AddJsonFile(path: "appsettings.json");
    //            config.AddJsonFile(path: $"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true);
    //            config.AddInMemoryCollection(_appSettings);
    //        })
    //        .UseSerilog((_, loggerConfiguration) =>
    //        {
    //            loggerConfiguration
    //                .WriteTo.Sink<TestSink>()
    //                .MinimumLevel.Verbose()
    //                .MinimumLevel.Override(source: "Microsoft", LogEventLevel.Warning);
    //        })
    //        .UseStartup(typeof(Api.AssemblyMarker));
    //}
    
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
        });

        builder.ConfigureServices(services =>
        {
        });
    }
}