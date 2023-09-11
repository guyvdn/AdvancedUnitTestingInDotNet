using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

namespace WeatherService.Api.Configuration;

internal static class SwaggerConfiguration
{
    public static void AddSwaggerWithAuthentication(this IServiceCollection services, Action<SwaggerConfigurationOptions> options)
    {
        services.AddOptions<SwaggerConfigurationOptions>()
            .Configure(options)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var optionsValue = new SwaggerConfigurationOptions();
        options?.Invoke(optionsValue);

        services.AddSwaggerGen(o =>
        {
            var openApiOauthFlow = new OpenApiOAuthFlow()
            {
                TokenUrl = new Uri(optionsValue.TokenUrl),
            };

            o.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
            {
                Flows = new OpenApiOAuthFlows()
                {
                    ClientCredentials = openApiOauthFlow
                },
                In = ParameterLocation.Header,
                Name = HeaderNames.Authorization,
                Type = SecuritySchemeType.OAuth2
            });
            o.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Type = new ReferenceType?(ReferenceType.SecurityScheme),
                            Id = "oauth2"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    public static void UseSwaggerWithAuthentication(this IApplicationBuilder app, Action<SwaggerConfigurationOptions> options)
    {
        var optionsValue = new SwaggerConfigurationOptions();
        options?.Invoke(optionsValue);

        app.UseSwagger();

        app.UseSwaggerUI(o =>
        {
            o.OAuthClientId(optionsValue.ClientId);
            o.OAuthClientSecret(optionsValue.ClientSecret);
        });
    }
}