using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using WeatherService.Testing.NUnit.Unit.Controllers.Specifications;

namespace WeatherService.Testing.NUnit.Unit.Controllers.ControllerTests;

[TestFixture]
internal sealed class AllControllersTests
{
    private List<Type> _testedControllers = default!;

    private static IEnumerable<TestCaseData> AllControllers()
    {
        var controllerBaseType = typeof(ControllerBase);

        return typeof(Api.AssemblyMarker).Assembly.GetTypes()
            .Where(t => !t.IsAbstract && controllerBaseType.IsAssignableFrom(t))
            .ToList().ConvertAll(t => new TestCaseData(t).SetName(t.FullName));
    }

    [OneTimeSetUp]
    public void Setup()
    {
        var controllerTestSpecificationType = typeof(ControllerTestSpecification<>);

        _testedControllers = GetType().Assembly.GetTypes()
            .Where(t => t is { IsAbstract: false, BaseType.IsGenericType: true } && 
                        t.BaseType.GetGenericTypeDefinition() == controllerTestSpecificationType)
            .Select(t => t.BaseType!.GenericTypeArguments[0])
            .ToList();
    }
    
    [TestCaseSource(nameof(AllControllers))]
    public void Should_be_tested(Type controllerType)
    {
        _testedControllers.Should().Contain(controllerType);
    }
}