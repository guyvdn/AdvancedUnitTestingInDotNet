using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WeatherService.Testing.XUnit.Integration.Features;
public abstract class TestSpecification<TController, TRequest, TResponse> : WeatherService.Testing.XUnit.Integration.Core.Specifications.TestSpecification<TController, TRequest, TResponse>
    where TController : ControllerBase
    where TRequest : IBaseRequest
{
    public override async Task InitializeAsync()
    {
        await TestSetupFixture.ExecuteAsync();
        await base.InitializeAsync();
    }
}
public abstract class TestSpecification<TController, TRequest> : WeatherService.Testing.XUnit.Integration.Core.Specifications.TestSpecification<TController, TRequest>
    where TController : ControllerBase
    where TRequest : IBaseRequest
{
    public override async Task InitializeAsync()
    {
        await TestSetupFixture.ExecuteAsync();
        await base.InitializeAsync();
    }
}

public abstract class TestSpecification : WeatherService.Testing.XUnit.Integration.Core.Specifications.TestSpecification
{
    public override async Task InitializeAsync()
    {
        await TestSetupFixture.ExecuteAsync();
        await base.InitializeAsync();
    }
}