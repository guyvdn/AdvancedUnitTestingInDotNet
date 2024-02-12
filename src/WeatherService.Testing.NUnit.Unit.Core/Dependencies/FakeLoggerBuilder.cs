using Microsoft.Extensions.Logging;

namespace WeatherService.Testing.NUnit.Unit.Core.Dependencies;

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