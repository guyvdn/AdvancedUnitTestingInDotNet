using WeatherService.Testing.Unit.Core.Customizations;
using WeatherService.Testing.Unit.Core.Dependencies;

namespace WeatherService.Testing.Unit.Core.Specifications;

[TestFixture]
public abstract class TestSpecificationBase
{
    protected static Fixture Fixture { get; }

    static TestSpecificationBase()
    {
        Fixture = new Fixture().WithCustomizations();
    }
}

public abstract class TestSpecification : TestSpecificationBase
{
    [OneTimeSetUp]
    public virtual async Task OneTimeSetUp()
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

    [TearDown]
    public async Task BaseTearDown()
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

    [OneTimeSetUp]
    public override async Task OneTimeSetUp()
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
            TestContext.WriteLine(entry.Message);
        }
    }
}