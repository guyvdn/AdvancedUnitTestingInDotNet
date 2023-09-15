using FluentValidation.Results;
using MediatR.Pipeline;

namespace WeatherService.Core.Infrastructure;

internal sealed class ValidationBehavior<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
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
            throw new FluentValidation.ValidationException($"Error validating {typeof(TRequest).Name}", failures);
        }
    }
}