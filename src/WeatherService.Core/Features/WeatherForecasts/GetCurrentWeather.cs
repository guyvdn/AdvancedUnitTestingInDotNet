using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using WeatherService.Core.Features.AuditLogs.Models;
using WeatherService.Core.Features.WeatherForecasts.Models;

namespace WeatherService.Core.Features.WeatherForecasts;

public static class GetCurrentWeather
{
    public sealed class Request : IRequest<WeatherResponse>
    {
        public required string City { get; init; }
    }

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.City).NotEmpty();
        }
    }  

    internal sealed class Handler : IRequestHandler<Request, WeatherResponse>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWeatherApiClient _weatherApiClient;
        private readonly IWeatherRepository _weatherRepository;

        public Handler(
            IHttpContextAccessor httpContextAccessor,
            IWeatherApiClient weatherApiClient,
            IWeatherRepository weatherRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _weatherApiClient = weatherApiClient;
            _weatherRepository = weatherRepository;
        }

        public async Task<WeatherResponse> Handle(Request request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            await _weatherRepository.AddAuditLogAsync(new AuditLog
            {
                NameIdentifier = user,
                Message = AuditLogMessage.GetCurrentWeatherWasCalledForCity(request.City),
                TimeStampUTC = DateTime.UtcNow
            }, cancellationToken);

            return await _weatherApiClient.GetCurrentWeatherAsync(request.City, cancellationToken);
        }
    }
}