using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WeatherService.Testing.NUnit.ServiceRegistrations.Extensions;

namespace WeatherService.Testing.NUnit.ServiceRegistrations.Specifications;

[TestFixture]
public abstract class ServiceCollectionTestSpecification<TAssemblyMarker>
{
    private ServiceCollection _services;

    [SetUp]
    public void Setup()
    {
        _services = new ServiceCollection();
        AddServices(_services);
    }

    protected abstract void AddServices(ServiceCollection services);

    private static IEnumerable<Type> TypesToTest()
    {
        return typeof(TAssemblyMarker).Assembly
            .GetTypes()
            .Where(t => !TypesToSkip().Contains(t) &&
                        t.HasConstructor() &&
                        !t.IsAbstract &&
                        !t.IsCompilerGenerated()
            );
    }

    private static IEnumerable<Type> TypesToSkip()
    {
        return Array.Empty<Type>();
    }

    [Test, TestCaseSource(nameof(TypesToTest))]
    public void It_should_be_able_to_resolve_all_dependencies(Type type)
    {
        TestContext.WriteLine($"Validating {type.FullName}");

        if (type.IsMediatRRequest() || type.IsMediatRNotification())
        {
            MediatRHandlerShouldBeRegisteredFor(type);
            return;
        }

        var constructor = type.GetConstructors().FirstOrDefault();

        foreach (var parameter in constructor!.GetParameters())
        {
            var parameterType = parameter.ParameterType;

            if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(ILogger<>))
            {
                var genericType = parameterType.GetGenericArguments()[0];
                genericType.Should().Be(type, "Logger should be of same type as instance type");
                continue;
            }

            if (typeof(HttpClient).IsAssignableFrom(parameterType))
            {
                _services
                    .Where(x => x.ServiceType == typeof(IHttpClientFactory))
                    .Should().HaveCountGreaterThan(0, $"AddHttpClient should be used to register {type.Name}");

                continue;
            }

            if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(IOptions<>))
            {
                var optionsType = parameterType.GetGenericArguments()[0];
                var optionsConfiguration = typeof(IConfigureOptions<>).MakeGenericType(optionsType);

                _services
                    .Where(x => x.ServiceType == optionsConfiguration)
                    .Should().HaveCountGreaterThan(0, $"No configuration for options '{optionsType.Name}' found");

                continue;
            }

            if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                parameterType = parameterType.GetGenericArguments()[0];
            }

            var services = new List<ServiceDescriptor>();

            if (parameterType.IsGenericType)
            {
                parameterType = parameterType.GetGenericTypeDefinition();
                services = _services.Where(x => x.ServiceType.IsGenericType &&
                                                x.ServiceType.GetGenericTypeDefinition() == parameterType).ToList();
            }
            else
            {
                services = _services.Where(x => x.ServiceType == parameterType).ToList();
            }

            services.Should().HaveCountGreaterThan(0, $"No registration for type '{parameterType.Name}' found");

            foreach (var service in services)
            {
                if (service.ImplementationType is null)
                    continue;

                if (service.ImplementationType.IsDbContextFactory())
                    continue;

                if (service.ImplementationType.HasConstructor())
                    It_should_be_able_to_resolve_all_dependencies(service.ImplementationType);
            }
        }
    }

    private void MediatRHandlerShouldBeRegisteredFor(Type type)
    {
        var handlers = _services.Where(x => x.ServiceType.IsGenericType &&
                                            x.ServiceType.GetGenericTypeDefinition().IsMedatRHandler() &&
                                            x.ServiceType.GenericTypeArguments[0] == type);

        handlers.Should().NotBeEmpty($"A MediatR handler should be registered for {type.FullName}");
    }
}