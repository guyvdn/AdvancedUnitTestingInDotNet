using WeatherService.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();

//builder.Services.AddAuthorization();
builder.Services.AddJwtAuthentication(o => builder.Configuration.GetSection(nameof(AuthenticationConfigurationOptions)).Bind(o));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerWithAuthentication(o => builder.Configuration.GetSection(nameof(SwaggerConfigurationOptions)).Bind(o));

builder.Services.AddWeatherServiceCore(
    builder.Configuration.GetConnectionString("WeatherApiDb")!, 
    o => builder.Configuration.GetSection(nameof(WeatherApiOptions)).Bind(o));

var app = builder.Build();

//var options = app.Services.GetRequiredService<IOptions<WeatherApiOptions>>().Value;

app.UseSwaggerWithAuthentication(o => app.Configuration.GetSection(nameof(SwaggerConfigurationOptions)).Bind(o));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();