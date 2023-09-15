using WeatherService.Testing.NUnit.Core.Mocking;

namespace WeatherService.Testing.NUnit.Core.Specifications;

public abstract class TestSpecification<TSubjectUnderTest> : TestSpecification
{
    protected TSubjectUnderTest Sut { get; private set; }

    [SetUp]
    public override async Task SetUp()
    {
        BaseSetUp();
        await ArrangeAsync();
        Sut = CreateSut();
        await ActAsync();
    }

    protected virtual TSubjectUnderTest CreateSut()
    {
        return Scope.CreateSut<TSubjectUnderTest>();
    }
}

public abstract class TestSpecification : TestSpecificationBase
{
    protected IMockScope Scope { get; private set; }

    [SetUp]
    public virtual async Task SetUp()
    {
        BaseSetUp();
        await ArrangeAsync();
        await ActAsync();
    }

    protected virtual void BaseSetUp()
    {
        //Scope = new MockScope();
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
        await TearDownAsync();

        //if (TestContext.CurrentContext.Result.Outcome == ResultState.Success)
        //{
        //    Scope.Dispose();
        //}
        //else if (Scope.TryGetDependency<LoggerFake>(out var logger))
        //{
        //    foreach (var messageEntry in logger.Messages)
        //    {
        //        TestContext.WriteLine(messageEntry.Message);
        //    }
        //}
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