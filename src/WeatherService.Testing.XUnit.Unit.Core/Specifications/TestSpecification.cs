using WeatherService.Testing.XUnit.Unit.Core.Customizations;
using WeatherService.Testing.XUnit.Unit.Core.Dependencies;

namespace WeatherService.Testing.XUnit.Unit.Core.Specifications;

public abstract class TestSpecificationBase
{
    protected static Fixture Fixture { get; }

    static TestSpecificationBase()
    {
        Fixture = new Fixture().WithCustomizations();
    }
}

public abstract class TestSpecification : TestSpecificationBase, IAsyncLifetime
{
    public virtual async Task InitializeAsync()
    {
        await ArrangeAsync();
        await ActAsync();
    }

    protected virtual void Arrange()
    {
    }

    protected virtual Task ArrangeAsync()
    {
        Arrange();
        return Task.CompletedTask;
    }

    protected virtual void Act()
    {
    }

    protected virtual Task ActAsync()
    {
        Act();
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        TearDownInternal();
        await TearDownAsync();
    }

    internal virtual void TearDownInternal()
    {
    }

    protected virtual Task TearDownAsync()
    {
        TearDown();
        return Task.CompletedTask;
    }

    protected virtual void TearDown()
    {
    }
}

public abstract class TestSpecification<TSubjectUnderTest> : TestSpecification
{
    protected TSubjectUnderTest Sut { get; private set; }

    protected virtual object[] ExplicitDependencies { get; } = Array.Empty<object>();

    private DependencyList Dependencies { get; } = new();

    public override async Task InitializeAsync()
    {
        RegisterDependencyBuilders();
        CreateDependencies();
        await ArrangeAsync();
        Sut = CreateSut();
        await ActAsync();
    }

    protected virtual void RegisterDependencyBuilders()
    {
        Dependencies.RegisterDependencyBuilders<AssemblyMarker>();
    }

    protected virtual void CreateDependencies()
    {
        Dependencies.CreateFor<TSubjectUnderTest>(ExplicitDependencies);
    }

    protected virtual TSubjectUnderTest CreateSut()
    {
        return (TSubjectUnderTest)Activator.CreateInstance(typeof(TSubjectUnderTest), Dependencies.ToArray())!;
    }

    protected TDependency Dependency<TDependency>() where TDependency : class
    {
        if (!Dependencies.TryGet<TDependency>(out var dependency))
            throw new TestSpecificationException($"No dependency of type {typeof(TDependency).Name} found");

        return dependency;
    }

    internal override void TearDownInternal()
    {
        if (!Dependencies.TryGet<FakeLogger>(out var fakeLogger))
            return;

        foreach (var entry in fakeLogger.Messages)
        {
            Console.WriteLine(entry.Message);
        }
    }
}