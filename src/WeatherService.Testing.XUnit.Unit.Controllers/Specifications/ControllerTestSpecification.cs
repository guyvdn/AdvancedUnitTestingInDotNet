using System.Reflection;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace WeatherService.Testing.XUnit.Unit.Controllers.Specifications;

public abstract class ControllerTestSpecification<TController>
    where TController : ControllerBase
{
    public static TheoryData<MethodInfo> AllMethods()
    {
        return new TheoryData<MethodInfo>(typeof(TController)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
    }

    [Fact]
    public void Should_be_public()
    {
        typeof(TController).IsPublic.Should().BeTrue(" SwaggerGen will not find them otherwise");
    }

    [Fact]
    public void Should_be_abstract_or_sealed()
    {
        var isSealedOrAbstract = typeof(TController).IsSealed || 
                                 typeof(TController).IsAbstract;

        isSealedOrAbstract.Should().BeTrue(" we care about performance");
    }

    [Fact]
    public void Should_have_explicit_Authorization()
    {
        typeof(TController).GetCustomAttributes().Should().Contain(x =>
            x.GetType().IsAssignableTo(typeof(AuthorizeAttribute)) ||
            x.GetType().IsAssignableTo(typeof(AllowAnonymousAttribute)), because: "safety first");
    }

    [Theory, MemberData(nameof(AllMethods))]
    public void All_methods_should_be_async_ActionResult(MethodInfo method)
    {
        var returnType = method.ReturnType;
        returnType.Should().BeAssignableTo(typeof(Task<IActionResult>), because: $"methods on {typeof(TController)} should be async so that the call can be aborted when needed");

        var parameters = method.GetParameters();
        parameters.Should().Contain(x => x.ParameterType.IsAssignableTo(typeof(CancellationToken)), because: "it should pass the CancellationToken to underlying methods");
    }

    [Theory, MemberData(nameof(AllMethods))]
    public void All_methods_should_specify_at_least_one_ProducesResponseTypeAttribute(MethodInfo method)
    {
        var responseTypeAttributes =
            method.CustomAttributes.Where(x => x.AttributeType == typeof(ProducesResponseTypeAttribute)).ToList();

        responseTypeAttributes.Should().NotBeEmpty(because: $"it helps the consumer to know what StatusCode to expect when calling {method.Name} method on {typeof(TController)} ");

        using var scope = new AssertionScope();

        foreach (var attribute in responseTypeAttributes)
        {
            var arguments = new List<CustomAttributeTypedArgument>();
            arguments.AddRange(attribute.ConstructorArguments);
            arguments.AddRange(attribute.NamedArguments.Select(x => x.TypedValue));

            if (arguments.Count == 1 && arguments.Single().Value is StatusCodes.Status404NotFound)
                continue;
            
            if (IsHttpGet(method))
            {
                arguments.Should().Contain(x => x.ArgumentType == typeof(Type))
                    .Which.Value.Should().NotBe(typeof(void), 
                        because: "the return type should be defined so that it will show in Swagger documentation");
            }
        }
    }

    private static bool IsHttpGet(MemberInfo method)
    {
        return method.CustomAttributes.Any(x => x.AttributeType == typeof(HttpGetAttribute));
    }
}