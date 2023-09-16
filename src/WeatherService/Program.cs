using WeatherService.Api.Configuration;
using WeatherService.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//builder.Services.AddAuthorization();
builder.Services.AddJwtAuthentication(o => builder.Configuration.GetSection(nameof(AuthenticationConfigurationOptions)).Bind(o));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerWithAuthentication(o => builder.Configuration.GetSection(nameof(SwaggerConfigurationOptions)).Bind(o));

builder.Services.AddWeatherServiceCore(
    builder.Configuration.GetConnectionString("WeatherApiDb")!, 
    o => builder.Configuration.GetSection(nameof(WeatherApiOptions)).Bind(o));

var app = builder.Build();

app.UseSwaggerWithAuthentication(o => app.Configuration.GetSection(nameof(SwaggerConfigurationOptions)).Bind(o));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();