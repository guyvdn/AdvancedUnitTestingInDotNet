using Microsoft.Extensions.Logging;
using WeatherService.Core.Services;

namespace WeatherService.Core.Features.Images;

public static class AddImage
{
    public sealed class Request: IRequest
    {
        public required int ConditionCode { get; init; }

        public required string Base64Content { get; init; }
    }

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.ConditionCode).GreaterThan(0);

            RuleFor(x => x.Base64Content).NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Request>
    {
        private readonly IFileNameService _fileNameService;
        private readonly IFileService _fileService;
        private readonly ILogger<Handler> _logger;

        public Handler(IFileNameService fileNameService, IFileService fileService, ILogger<Handler> logger)
        {
            _fileNameService = fileNameService;
            _fileService = fileService;
            _logger = logger;
        }

        public async Task Handle(Request request, CancellationToken cancellationToken)
        {
            var fileName = _fileNameService.GetImageName(request.ConditionCode);
            
            _logger.LogInformation("Creating image for {ConditionCode} with name {FileName}", request.ConditionCode,fileName);
            
            var fileContent = Convert.FromBase64String(request.Base64Content);

            await _fileService.SaveFileAsync(fileName, fileContent, cancellationToken);
        }
    }
}