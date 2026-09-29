using Microsoft.AspNetCore.Mvc;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Application.Features.Testimonials.Commands;
using ShahidPortfolio.Application.Features.Testimonials.Queries.GetTestimonials;

namespace ShahidPortfolio.API.Controllers;

public class TestimonialsController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<TestimonialDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTestimonials(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetTestimonialsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost("submit")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SubmitTestimonial([FromBody] SubmitPublicTestimonialCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}
