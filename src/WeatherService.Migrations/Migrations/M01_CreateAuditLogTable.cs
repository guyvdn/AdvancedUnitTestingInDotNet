using FluentMigrator;

namespace WeatherService.Migrations.Migrations;

[Migration(1)]
public class M01_CreateAuditLogTable : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("AuditLog").InSchema("dbo") 
            .WithColumn("AuditLogId").AsInt32().PrimaryKey().Identity().NotNullable()
            .WithColumn("NameIdentifier").AsAnsiString(50).NotNullable()
            .WithColumn("Message").AsAnsiString(100).NotNullable()
            .WithColumn("TimeStampUTC").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentUTCDateTime);
    }
}