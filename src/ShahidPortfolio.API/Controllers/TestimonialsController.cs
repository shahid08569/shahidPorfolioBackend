using Microsoft.AspNetCore.Mvc;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Application.Features.Testimonials.Queries.GetTestimonials;

namespace ShahidPortfolio.API.Controllers;

public class TestimonialsController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<TestimonialDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTestimonials()
    {
        var result = await Mediator.Send(new GetTestimonialsQuery());
        return Ok(result);
    }
}
