using NSubstitute;
using NSubstitute.Core;

namespace WeatherService.Testing.Integration.Core.Specifications;

public abstract partial class TestSpecification
{
    private void SetupDependencies()
    {
        MockHttpClient();
    }

    private void ResetDependencies()
    {
        TestSink.ClearAllEvents();

        foreach (var dependency in _dependencies.Values.OfType<ICallRouterProvider>())
        {
            dependency.ClearReceivedCalls<object>();
        }
    }

    private void AddDependency<T>(T mock)
    {
        _dependencies.Add(typeof(T), mock!);
    }

    private T AddDependency<T>()
        where T : class
    {
        var mock = Substitute.For<T>();
        AddDependency<T>(mock);
        return mock;
    }
}