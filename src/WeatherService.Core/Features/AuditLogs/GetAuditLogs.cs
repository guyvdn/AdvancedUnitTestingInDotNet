using WeatherService.Core.Features.AuditLogs.Models;

namespace WeatherService.Core.Features.AuditLogs;

public static class GetAuditLogs
{
    public sealed class Request : IRequest<List<AuditLog>>
    {
        public required int NumberOfDays { get; init; }
    }

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.NumberOfDays).GreaterThan(0);
            RuleFor(x => x.NumberOfDays).LessThanOrEqualTo(7);
        }
    }

    public sealed class Handler : IRequestHandler<Request, List<AuditLog>>
    {
        private readonly IWeatherRepository _weatherRepository;

        public Handler(IWeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;
        }

        public Task<List<AuditLog>> Handle(Request request, CancellationToken cancellationToken)
        {
            return _weatherRepository.GetAuditLogsAsync(request.NumberOfDays, cancellationToken);
        }
    }
}