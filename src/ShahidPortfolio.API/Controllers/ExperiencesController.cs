using Microsoft.AspNetCore.Mvc;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Application.Features.Experiences.Queries.GetTimeline;

namespace ShahidPortfolio.API.Controllers;

public class ExperiencesController : BaseApiController
{
    [HttpGet("timeline")]
    [ProducesResponseType(typeof(ApiResponse<TimelineDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTimeline()
    {
        var result = await Mediator.Send(new GetTimelineQuery());
        return Ok(result);
    }
}
