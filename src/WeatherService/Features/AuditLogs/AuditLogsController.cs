using WeatherService.Core.Features.AuditLogs;

namespace WeatherService.Api.Features.AuditLogs;

[Authorize]
[ApiController]
[Route("[controller]")]
public sealed class AuditLogsController : ControllerBase
{
    private readonly ISender _sender;

    public AuditLogsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet(Name = "GetAuditLogs")]
    [ProducesResponseType(typeof(ProblemDetails), Status400BadRequest)]
    [ProducesResponseType(typeof(Representation.AuditLog), Status200OK)]
    public async Task<IActionResult> Get(int numberOfDays, CancellationToken cancellationToken) // TODO Representation
    {
        var request = new GetAuditLogs.Request
        {
            NumberOfDays = numberOfDays
        };

        var auditLogs = await _sender.Send(request, cancellationToken);

        return Ok(auditLogs.ConvertAll(log => log.ToRepresentation()));
    }
}