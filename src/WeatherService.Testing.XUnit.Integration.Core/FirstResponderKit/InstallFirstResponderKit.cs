using FluentMigrator;

namespace WeatherService.Testing.XUnit.Integration.Core.FirstResponderKit;

[Migration(1_000_000)]
public sealed class InstallFirstResponderKit: ForwardOnlyMigration
{
    public override void Up()
    {
        Execute.Script("FirstResponderKit/sp_BlitzIndex.sql");
    }
}