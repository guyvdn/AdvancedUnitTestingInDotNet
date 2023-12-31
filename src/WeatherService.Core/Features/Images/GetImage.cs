﻿using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;
using WeatherService.Core.Services;

namespace WeatherService.Core.Features.Images;

public static class GetImage
{
    public sealed class Request: IRequest<OneOf<string, NotFound>>
    {
        public required int ConditionCode { get; init; }
    }

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.ConditionCode).GreaterThan(0);
        }
    }

    internal sealed class Handler : IRequestHandler<Request, OneOf<string, NotFound>>
    {
        private readonly IFileNameService _fileNameService;
        private readonly IFileService _fileService;
        private readonly ILogger<Handler> _logger;

        public Handler(
            IFileNameService fileNameService, 
            IFileService fileService,
            ILogger<Handler> logger)
        {
            _fileNameService = fileNameService;
            _fileService = fileService;
            _logger = logger;
        }

        public async Task<OneOf<string, NotFound>> Handle(Request request, CancellationToken cancellationToken)
        {
            var fileName = _fileNameService.GetImageName(request.ConditionCode);

            _logger.LogInformation("Getting image for {ConditionCode} with name {FileName}", request.ConditionCode,fileName);

            var bytes = await _fileService.GetFileAsync(fileName, cancellationToken);

            return bytes.Match<OneOf<string, NotFound>>(
                b => Convert.ToBase64String(b),
                notFound => notFound);
        }
    }
}