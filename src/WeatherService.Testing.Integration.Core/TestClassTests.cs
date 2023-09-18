using FluentMigrator;
using WeatherService.Testing.Unit.Core.Specifications;

namespace WeatherService.Testing.Integration.Core;

internal sealed class TestClassTests : TestClassesTestSpecification<AssemblyMarker>
{
    [TestCaseSource(nameof(TestClasses))]
    public override void All_test_classes_should_be_internal(Type type)
    {
        var migrationType = typeof(Migration);
        if (type.IsAssignableTo(migrationType))
            Assert.Ignore("Migrations should be public");

        base.All_test_classes_should_be_internal(type);
    }
}