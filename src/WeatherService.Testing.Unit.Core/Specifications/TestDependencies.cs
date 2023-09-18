using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace WeatherService.Testing.Unit.Core.Specifications;

public sealed class TestDependencies : IEnumerable<object>
{
    private readonly List<IDependencyBuilder> _dependencyBuilders = new();

    private readonly List<object> _dependencies = new();

    public void RegisterDependencyBuilders<TAssemblyMarker>()
    {
        var dependencyBuilderTypes = typeof(TAssemblyMarker).Assembly
            .GetTypes().Where(IsDependencyBuilder);

        foreach (var builderType in dependencyBuilderTypes)
        {
            var builder = (IDependencyBuilder)Activator.CreateInstance(builderType)!;
            _dependencyBuilders.Add(builder);
        }
    }

    private static bool IsDependencyBuilder(Type x)
    {
        return !x.IsAbstract && x.IsAssignableTo(typeof(IDependencyBuilder));
    }

    public void CreateFor<TSubjectUnderTest>(params object[] explicitDependencies)
    {
        var typeOfSubjectUnderTest = typeof(TSubjectUnderTest);
        var constructors = typeOfSubjectUnderTest.GetConstructors();

        var constructor = constructors.FirstOrDefault() ??
                          throw new TestSpecificationException($"No constructor found for type {typeOfSubjectUnderTest.Name}");

        foreach (var parameter in constructor.GetParameters())
        {
            var parameterType = parameter.ParameterType;
            var depenendency = Array.Find(explicitDependencies, parameterType.IsInstanceOfType);

            for (var i = 0; i < _dependencyBuilders.Count && depenendency is null; i++)
            {
                depenendency = _dependencyBuilders[i].Build(parameterType);
            }

            _dependencies.Add(depenendency ?? CreateDependency(parameterType));
        }
    }

    private static object CreateDependency(Type type)
    {
        return Substitute.For(typesToProxy: new[] { type }, constructorArguments: Array.Empty<object>());
    }

    public IEnumerator<object> GetEnumerator() => _dependencies.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool TryGet<TDependency>([NotNullWhen(true)] out TDependency? dependency)
    {
        dependency = _dependencies.OfType<TDependency>().SingleOrDefault();
        return dependency is not null;
    }
}