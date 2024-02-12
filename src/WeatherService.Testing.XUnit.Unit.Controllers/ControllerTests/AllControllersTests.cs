using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using WeatherService.Testing.XUnit.Unit.Controllers.Specifications;
using WeatherService.Testing.XUnit.Unit.Core.Specifications;
using Xunit;

namespace WeatherService.Testing.XUnit.Unit.Controllers.ControllerTests;

public sealed class AllControllersTests: TestSpecification
{
    private List<Type> _testedControllers = default!;

    public static TheoryData<Type> AllControllers()
    {
        var controllerBaseType = typeof(ControllerBase);

        return new TheoryData<Type>(typeof(Api.AssemblyMarker).Assembly.GetTypes()
            .Where(t => !t.IsAbstract && controllerBaseType.IsAssignableFrom(t)));
    }

    protected override void Arrange()
    {
        var controllerTestSpecificationType = typeof(ControllerTestSpecification<>);

        _testedControllers = GetType().Assembly.GetTypes()
            .Where(t => t is { IsAbstract: false, BaseType.IsGenericType: true } &&
                        t.BaseType.GetGenericTypeDefinition() == controllerTestSpecificationType)
            .Select(t => t.BaseType!.GenericTypeArguments[0])
            .ToList();
    }
    
    [Theory, MemberData(nameof(AllControllers))]
    public void Should_be_tested(Type controllerType)
    {
        _testedControllers.Should().Contain(controllerType);
    }
}