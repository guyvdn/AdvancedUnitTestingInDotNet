using WeatherService.Core.Features.Images;
using WeatherService.Representation;

namespace WeatherService.Api.Features.Images;

[Authorize]
[ApiController]
[Route("[controller]")]
public sealed class ImagesController : ControllerBase
{
    private readonly ISender _sender;

    public ImagesController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Get an image
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetImage")]
    public async Task<IActionResult> Get(int conditionCode, CancellationToken cancellationToken)
    {
        var request = new GetImage.Request
        {
            ConditionCode = conditionCode
        };

        var result = await _sender.Send(request, cancellationToken);

        return result.Match<IActionResult>(
            content => Ok(new Image
            {
                ConditionCode = request.ConditionCode,
                Base64Content = content,
            }),
            notFound => NotFound()
        );
    }

    /// <summary>
    /// Add an image
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// Sample request :
    ///
    ///     POST /images
    ///     {
    ///         "ConditionCode": 123,
    ///         "Base64Content": "AgQGCAoMDhASFA=="
    ///     }
    /// 
    /// </remarks>
    [HttpPost(Name = "AddImage")]
    public async Task<IActionResult> Add([FromBody] Image image, CancellationToken cancellationToken)
    {
        await _sender.Send(image.ToRequest(), cancellationToken);

        return CreatedAtAction(nameof(Get), new { conditionCode = image.ConditionCode }, image);
    }
}