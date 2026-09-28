using Microsoft.AspNetCore.Mvc;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Application.Features.Projects.Queries.GetProjectBySlug;
using ShahidPortfolio.Application.Features.Projects.Queries.GetProjects;

namespace ShahidPortfolio.API.Controllers;

public class ProjectsController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<ProjectCardDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProjects([FromQuery] bool? isFeatured, [FromQuery] string? tag)
    {
        var result = await Mediator.Send(new GetProjectsQuery(isFeatured, tag));
        return Ok(result);
    }

    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(ApiResponse<ProjectDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProjectBySlug(string slug)
    {
        var result = await Mediator.Send(new GetProjectBySlugQuery(slug));
        return Ok(result);
    }
}
