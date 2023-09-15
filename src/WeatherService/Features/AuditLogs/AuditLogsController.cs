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
    public async Task<IActionResult> Get(int numberOfDays, CancellationToken cancellationToken) // TODO Representation
    {
        var request = new GetAuditLogs.Request
        {
            NumberOfDays = numberOfDays
        };

        return Ok(await _sender.Send(request, cancellationToken));
    }
}