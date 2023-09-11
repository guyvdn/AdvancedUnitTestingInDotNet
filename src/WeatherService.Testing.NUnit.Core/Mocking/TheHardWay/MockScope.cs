//namespace WeatherService.Testing.NUnit.Core.Mocking.TheHardWay;

//internal sealed class MockScope : IMockScope
//{
//    private static readonly List<IDependencyBuilder> _dependencyBuilders = new();
//    private readonly List<object> _dependencies = new();

//    public MockScope()
//    {
//        AddDependencyBuilders<AssemblyMarker>();
//    }

//    public TSubjectUnderTest CreateSut<TSubjectUnderTest>(params object[] explicitDependencies)
//    {
//        var typeOfSubjectUnderTest = typeof(TSubjectUnderTest);
//        var constructor = typeOfSubjectUnderTest.GetConstructors().FirstOrDefault();
//        constructor.Should().NotBeNull(because: $"No constructor found for type {typeOfSubjectUnderTest.Name}");

//        foreach (var parameter in constructor!.GetParameters())
//        {
//            var parameterType = parameter.ParameterType;
//            var depenendency = Array.Find(explicitDependencies, parameterType.IsInstanceOfType);

//            for (var i = 0; i < _dependencyBuilders.Count && depenendency is null; i++)
//            {
//                depenendency = _dependencyBuilders[i].Build(parameterType);
//            }

//            _dependencies.Add(depenendency ?? Get(parameterType));
//        }

//        return (TSubjectUnderTest)Activator.CreateInstance(typeof(TSubjectUnderTest), _dependencies.ToArray())!;
//    }

//    public T Get<T>()
//        where T : class
//    {
//        return (T)Get(typeof(T));
//    }

//    private object Get(Type type)
//    {
//        if (_dependencies.TryGetValue(type, out var mock))
//        {
//            return mock;
//        }

//        return CreateMock(type);
//    }

//    public static void AddDependencyBuilders<TAssemblyMarker>()
//    {
//        static bool IsDependencyBuilder(Type x)
//        {
//            return !x.IsAbstract && x.IsAssignableTo(typeof(IDependencyBuilder));
//        }

//        var dependencyBuilderTypes = typeof(TAssemblyMarker).Assembly
//            .GetTypes().Where(IsDependencyBuilder);

//        foreach (var builderType in dependencyBuilderTypes)
//        {
//            var builder = (IDependencyBuilder)Activator.CreateInstance(builderType)!;
//            _dependencyBuilders.Add(builder);
//        }
//    }
//}

//public interface IDependencyBuilder
//{
//    public object? Build(Type type);
//}

//public abstract class DependencyBuilder<TDependency, TImplementation> : DependencyBuilder<TDependency>
//    where TImplementation : TDependency, new()
//{
//    protected override TDependency BuildInternal()
//    {
//        return new TImplementation();
//    }
//}

//public abstract class DependencyBuilder<TDependency> : IDependencyBuilder
//{
//    public object? Build(Type type)
//    {
//        if (type.IsAssignableTo(typeof(TDependency)))
//        {
//            return BuildInternal();
//        }

//        return null;
//    }

//    protected abstract TDependency BuildInternal();
//}