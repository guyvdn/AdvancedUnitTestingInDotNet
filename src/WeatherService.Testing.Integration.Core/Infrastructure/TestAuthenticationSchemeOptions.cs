using Microsoft.AspNetCore.Authentication;

namespace WeatherService.Testing.Integration.Core.Infrastructure;

internal sealed class TestAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public bool RequestIsAuthenticated { get; set; }

    public string UserName { get; set; } = "Unknown";
}