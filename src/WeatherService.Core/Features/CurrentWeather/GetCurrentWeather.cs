using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using WeatherService.Core.Features.AuditLog;

namespace WeatherService.Core.Features.CurrentWeather;

public static class GetCurrentWeather
{
    public sealed class Request : IRequest<WeatherResponse>
    {
        public required string? City { get; set; }
    }

    internal sealed class Validator
    {
        // TODO
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
            
            await _weatherRepository.AddAuditLogAsync(new AuditLog.AuditLog
            {
                NameIdentifier = user,
                Message = AuditLogMessage.GetCurrentWeatherWasCalledForCity(request.City),
                TimeStampUTC = DateTime.UtcNow
            }, cancellationToken);

            return await _weatherApiClient.GetCurrentWeatherAsync(request.City, cancellationToken);
        }
    }
}