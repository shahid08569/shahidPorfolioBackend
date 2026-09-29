using Microsoft.AspNetCore.Mvc;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Application.Features.Certificates.Queries;

namespace ShahidPortfolio.API.Controllers;

public class CertificatesController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<CertificateDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCertificates(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetCertificatesQuery(IncludeInactive: false), cancellationToken);
        return Ok(result);
    }
}
