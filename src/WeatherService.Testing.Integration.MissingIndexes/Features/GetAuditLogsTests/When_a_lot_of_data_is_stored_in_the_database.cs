using WeatherService.Core.DatabaseConfiguration.DbContexts;
using WeatherService.Representation;
using WeatherService.Testing.Integration.Core;
using WeatherService.Testing.Integration.Core.FirstResponderKit;
using WeatherService.Testing.Integration.Core.Infrastructure.Database;
using WeatherService.Testing.Integration.Core.Specifications;
using WeatherService.Testing.Integration.Seeding.Fakers;

namespace WeatherService.Testing.Integration.Seeding.Features.GetAuditLogsTests;

/// <summary>
/// <see cref="InstallFirstResponderKit"/>
/// </summary>
[Explicit("LongRunning")]
[Category("LongRunning")]
internal sealed class When_a_lot_of_data_is_stored_in_the_database
    : TestSpecification
{
    private IEnumerable<AuditLog>? _response;

    protected override async Task ArrangeAsync()
    {
        var auditLogFaker = new AuditLogFaker()
            .RuleFor(x => x.AuditLogId, _ => 0)
            .UseSeed(12345);

        await Seed.WithDbContextAsync<WeatherApiDbContext>(async db =>
        {
            await db.AuditLogs.AddRangeAsync(auditLogFaker.Generate(1_000));
        });
    }

    protected override async Task ActAsync()
    {
        var cancellationToken = Build.CancellationToken(5.Minutes());

        _response = await Client.GetFromJsonAsync<IEnumerable<AuditLog>>("AuditLogs?numberOfDays=7", cancellationToken);
    }

    [Test]
    public void Should_return_data()
    {
        _response.Should().NotBeEmpty();
    }

    [Test]
    public void Should_not_have_missing_indexes()
    {
        const int CommunityMessage = 1;

        var result = SqlHelper
            .ExecuteReader("EXEC [dbo].[sp_BlitzIndex] @ThresholdMB = 1, @Mode = 3", DatabaseContext.Current.ConnectionString)
            .Skip(CommunityMessage);

        result.Should().BeEmpty();

        // Obove call will fail with this message
        Assert.Fail("""
                    Expected result to be empty, but found {{"UnitTests", "dbo", "AuditLog", 584998M, "INEQUALITY:
                    [TimeStampUTC]  {datetime} INCLUDE:  [NameIdentifier]  {varchar(50)}, [Message]  {varchar(100)}
                    ", 306.2818M, 76.4M, 1L, 0L, 1L, , " [TimeStampUTC]  {datetime}", " [NameIdentifier]
                    {varchar(50)}, [Message]  {varchar(100)}", "1 use; Impact: 76.4%; Avg query cost: 306.2818",
                    -----------------------------------------------------------------------------------------------
                    "CREATE INDEX [TimeStampUTC_Includes] ON [UnitTests].[dbo].[AuditLog] ([TimeStampUTC])  INCLUDE
                    ([NameIdentifier], [Message]) WITH (FILLFACTOR=100, ONLINE=?, SORT_IN_TEMPDB=?,
                    -----------------------------------------------------------------------------------------------
                    DATA_COMPRESSION=?);", "EXEC dbo.sp_BlitzIndex @DatabaseName='UnitTests', @SchemaName='dbo',
                    @TableName='AuditLog';", 1, True, "<ShowPlanXML
                    xmlns="http://schemas.microsoft.com/sqlserver/2004/07/showplan" Version="1.539"
                    Build="15.0.4153.1"><BatchSequence><Batch><Statements><StmtSimple StatementText="(@__p_0
                    float)SELECT [a].[AuditLogId], [a].[Message], [a].[NameIdentifier],
                    [a].[TimeStampUTC]&#xD;&#xA;FROM [AuditLog] AS [a]&#xD;&#xA;WHERE [a].[TimeStampUTC] &gt;=
                    DATEADD(day, CAST(@__p_0 AS int), CONVERT(date, GETUTCDATE()))" StatementId="1"
                    StatementCompId="1" StatementType="SELECT" RetrievedFromCache="true"
                    StatementSubTreeCost="2.96995" StatementEstRows="112345" SecurityPolicyApplied="false"
                    StatementOptmLevel="TRIVIAL" QueryHash="0x3E387ED20C1991D8" QueryPlanHash="0x1853CE45B0EB2D2D"
                    CardinalityEstimationModelVersion="150"><StatementSetOptions QUOTED_IDENTIFIER="true"
                    ARITHABORT="false" CONCAT_NULL_YIELDS_NULL="true" ANSI_NULLS="true" ANSI_PADDING="true"
                    ANSI_WARNINGS="true" NUMERIC_ROUNDABORT="false" /><QueryPlan
                    NonParallelPlanReason="NoParallelPlansInDesktopOrExpressEdition" CachedPlanSize="24"
                    CompileTime="51" CompileCPU="49" CompileMemory="144"><MemoryGrantInfo SerialRequiredMemory="0"
                    SerialDesiredMemory="0" GrantedMemory="0" MaxUsedMemory="0"
                    /><OptimizerHardwareDependentProperties EstimatedAvailableMemoryGrant="414725"
                    EstimatedPagesCached="207362" EstimatedAvailableDegreeOfParallelism="4"
                    MaxCompileMemory="8562336" /><OptimizerStatsUsage><StatisticsInfo
                    LastUpdate="2023-09-14T20:30:27.20" ModificationCount="0" SamplingPercent="33.6515"
                    Statistics="[_WA_Sys_00000004_239E4DCF]" Table="[AuditLog]" Schema="[dbo]"
                    Database="[UnitTests]" /></OptimizerStatsUsage><TraceFlags IsCompileTime="1"><TraceFlag
                    Value="8017" Scope="Global" /></TraceFlags><RelOp NodeId="0" PhysicalOp="Clustered Index Scan"
                    LogicalOp="Clustered Index Scan" EstimateRows="112345" EstimatedRowsRead="200000"
                    EstimateIO="2.74979" EstimateCPU="0.220157" AvgRowSize="100" EstimatedTotalSubtreeCost="2.96995"
                    TableCardinality="200000" Parallel="0" EstimateRebinds="0" EstimateRewinds="0"
                    EstimatedExecutionMode="Row"><OutputList><ColumnReference Database="[UnitTests]" Schema="[dbo]"
                    Table="[AuditLog]" Alias="[a]" Column="AuditLogId" /><ColumnReference Database="[UnitTests]"
                    Schema="[dbo]" Table="[AuditLog]" Alias="[a]" Column="NameIdentifier" /><ColumnReference
                    Database="[UnitTests]" Schema="[dbo]" Table="[AuditLog]" Alias="[a]" Column="Message"
                    /><ColumnReference Database="[UnitTests]" Schema="[dbo]" Table="[AuditLog]" Alias="[a]"
                    Column="TimeStampUTC" /></OutputList><IndexScan Ordered="0" ForcedIndex="0" ForceScan="0"
                    NoExpandHint="0" Storage="RowStore"><DefinedValues><DefinedValue><ColumnReference
                    Database="[UnitTests]" Schema="[dbo]" Table="[AuditLog]" Alias="[a]" Column="AuditLogId"
                    /></DefinedValue><DefinedValue><ColumnReference Database="[UnitTests]" Schema="[dbo]"
                    Table="[AuditLog]" Alias="[a]" Column="NameIdentifier"
                    /></DefinedValue><DefinedValue><ColumnReference Database="[UnitTests]" Schema="[dbo]"
                    Table="[AuditLog]" Alias="[a]" Column="Message" /></DefinedValue><DefinedValue><ColumnReference
                    Database="[UnitTests]" Schema="[dbo]" Table="[AuditLog]" Alias="[a]" Column="TimeStampUTC"
                    /></DefinedValue></DefinedValues><Object Database="[UnitTests]" Schema="[dbo]"
                    Table="[AuditLog]" Index="[PK_AuditLog]" Alias="[a]" IndexKind="Clustered" Storage="RowStore"
                    /><Predicate><ScalarOperator ScalarString="[UnitTests].[dbo].[AuditLog].[TimeStampUTC] as
                    [a].[TimeStampUTC]&gt;=dateadd(day,CONVERT(int,[@__p_0],0),CONVERT(date,getutcdate(),0))"><Compare
                    CompareOp="GE"><ScalarOperator><Identifier><ColumnReference Database="[UnitTests]"
                    Schema="[dbo]" Table="[AuditLog]" Alias="[a]" Column="TimeStampUTC"
                    /></Identifier></ScalarOperator><ScalarOperator><Identifier><ColumnReference
                    Column="ConstExpr1001"><ScalarOperator><Intrinsic FunctionName="dateadd"><ScalarOperator><Const
                    ConstValue="(4)" /></ScalarOperator><ScalarOperator><Convert DataType="int" Style="0"
                    Implicit="0"><ScalarOperator><Identifier><ColumnReference Column="@__p_0"
                    /></Identifier></ScalarOperator></Convert></ScalarOperator><ScalarOperator><Convert
                    DataType="date" Style="0" Implicit="0"><ScalarOperator><Intrinsic FunctionName="getutcdate"
                    /></ScalarOperator></Convert></ScalarOperator></Intrinsic></ScalarOperator></ColumnReference></Identifier></ScalarOperator></Compare></ScalarOperator></Predicate></IndexScan></RelOp><ParameterList><ColumnReference
                    Column="@__p_0" ParameterDataType="float" ParameterCompiledValue="(-7.0000000000000000e+000)"
                    /></ParameterList></QueryPlan></StmtSimple></Statements></Batch></BatchSequence></ShowPlanXML>"}}.
                    """);
    }
}