using Bogus;

namespace WeatherService.Testing.Integration.Core.Features.GetAuditLogsTests;

internal class NlFaker<T> : Faker<T> where T : class
{
    protected NlFaker() : base("nl_BE")
    {
    }

    protected void WithPrivateConstructor()
    {
        CustomInstantiator(_ => (T)Activator.CreateInstance(typeof(T), nonPublic: true)!);
    }

    protected void WithFactoryMethod(Func<Faker, T> method)
    {
        CustomInstantiator(method);
    }
}