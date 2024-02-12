using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WeatherService.Testing.XUnit.ServiceRegistrations.Extensions;

internal static class TypeExtensions
{
    internal static bool HasConstructor(this Type type)
    {
        return type.GetConstructors().Any();
    }

    internal static bool IsDbContextFactory(this Type type)
    {
        return type.IsGenericType &&
               type.GetInterfaces().Any(i => i.IsGenericType &&
                                             i.GetGenericTypeDefinition() == typeof(IDbContextFactory<>));
    }

    internal static bool IsMediatRRequest(this Type type)
    {
        return typeof(IBaseRequest).IsAssignableFrom(type);
    }

    internal static bool IsMediatRNotification(this Type type)
    {
        return typeof(INotification).IsAssignableFrom(type);
    }

    internal static bool IsMedatRHandler(this Type type)
    {
        return typeof(IRequestHandler<>).IsAssignableFrom(type) ||
               typeof(IRequestHandler<,>).IsAssignableFrom(type) ||
               typeof(INotificationHandler<>).IsAssignableFrom(type);
    }

    internal static bool IsCompilerGenerated(this Type type)
    {
        return Attribute.GetCustomAttribute(type, typeof(CompilerGeneratedAttribute)) != null;
    }
}