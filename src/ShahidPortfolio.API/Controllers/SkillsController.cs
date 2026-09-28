using Microsoft.AspNetCore.Mvc;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Application.Features.Skills.Queries.GetSkills;

namespace ShahidPortfolio.API.Controllers;

public class SkillsController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<SkillCategoryGroupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSkills([FromQuery] bool? topSkillsOnly)
    {
        var result = await Mediator.Send(new GetSkillsQuery(topSkillsOnly));
        return Ok(result);
    }
}
