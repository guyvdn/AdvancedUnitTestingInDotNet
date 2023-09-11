using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WeatherService.Api.Configuration;

internal static class AuthenticationConfiguration
{
    public static void AddJwtAuthentication(this IServiceCollection services, Action<AuthenticationConfigurationOptions> options)
    {
        services.AddOptions<AuthenticationConfigurationOptions>()
            .Configure(options)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var optionsValue = new AuthenticationConfigurationOptions();
        options?.Invoke(optionsValue);

        services
            .AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.Authority = optionsValue.Authority;
                o.Audience = optionsValue.Audience;
            });
    }
}