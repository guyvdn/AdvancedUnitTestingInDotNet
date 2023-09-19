using FluentValidation.Results;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace WeatherService.Core.Infrastructure;

internal sealed class ValidationBehavior<TRequest> 
    : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehavior<TRequest>> _logger;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationBehavior<TRequest>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return;
        
        var failures = new List<ValidationFailure>();

        foreach (var validator in _validators)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            failures.AddRange(validationResult.Errors);
        }

        if (failures.Any())
        {
            _logger.LogInformation("Error validating {Request} with {Failures}", typeof(TRequest).Name, failures);
            throw new FluentValidation.ValidationException($"Error validating {typeof(TRequest).Name}", failures);
        }
    }
}