using Microsoft.AspNetCore.Mvc;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Application.Features.Settings.Queries.GetPublicSettings;

namespace ShahidPortfolio.API.Controllers;

public class SettingsController : BaseApiController
{
    [HttpGet("public")]
    [ProducesResponseType(typeof(ApiResponse<PublicSettingsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPublicSettings()
    {
        var result = await Mediator.Send(new GetPublicSettingsQuery());
        return Ok(result);
    }
}
