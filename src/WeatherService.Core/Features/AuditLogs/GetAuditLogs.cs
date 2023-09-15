using WeatherService.Core.Features.AuditLogs.Models;

namespace WeatherService.Core.Features.AuditLogs;

public static class GetAuditLogs
{
    public sealed class Request : IRequest<IEnumerable<AuditLog>>
    {
        public required int NumberOfDays { get; init; }
    }

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.NumberOfDays).GreaterThan(0);
            RuleFor(x => x.NumberOfDays).LessThanOrEqualTo(7);
        }
    }

    internal sealed class Handler : IRequestHandler<Request, IEnumerable<AuditLog>>
    {
        private readonly IWeatherRepository _weatherRepository;

        public Handler(IWeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;
        }

        public Task<IEnumerable<AuditLog>> Handle(Request request, CancellationToken cancellationToken)
        {
            return _weatherRepository.GetAuditLogsAsync(request.NumberOfDays, cancellationToken);
        }
    }
}