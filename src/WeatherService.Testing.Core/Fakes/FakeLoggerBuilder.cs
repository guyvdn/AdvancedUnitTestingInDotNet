using Microsoft.Extensions.Logging;
using WeatherService.Testing.Core.Specifications;

namespace WeatherService.Testing.Core.Fakes;

public sealed class FakeLoggerBuilder : IDependencyBuilder
{
    public object? Build(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableTo(typeof(ILogger<>)))
        {
            var loggerType = typeof(FakeLogger<>).MakeGenericType(type.GenericTypeArguments);
            return Activator.CreateInstance(loggerType);
        }

        return null;
    }
}