using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeatherService.Core.DatabaseConfiguration.Constants;
using WeatherService.Core.Features.AuditLog;

namespace WeatherService.Core.DatabaseConfiguration;

internal sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    /// <summary>
    /// Comment content of this method to see Configuration_should_match_sql_schema test in action
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLog");

        builder.Property(x => x.NameIdentifier)
            .HasColumnType(SqlType.VarChar)
            .HasMaxLength(50);

        builder.Property(x => x.Message)
            .HasColumnType(SqlType.VarChar)
            .HasConversion<AuditLogMessageConverter>()
            .HasMaxLength(100);

        builder.Property(x => x.TimeStampUTC)
            .HasColumnType(SqlType.DateTime) // Comment this line to demo IgnoreErrors
            .HasDefaultValueSql(SqlDefault.GetUtcDate);
    }
}