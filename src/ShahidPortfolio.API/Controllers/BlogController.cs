using Microsoft.AspNetCore.Mvc;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Application.Features.Blog.Queries.GetBlogPostBySlug;
using ShahidPortfolio.Application.Features.Blog.Queries.GetBlogPosts;

namespace ShahidPortfolio.API.Controllers;

public class BlogController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<BlogPostCardDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBlogPosts([FromQuery] string? tag)
    {
        var result = await Mediator.Send(new GetBlogPostsQuery(tag));
        return Ok(result);
    }

    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(ApiResponse<BlogPostDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBlogPostBySlug(string slug)
    {
        var result = await Mediator.Send(new GetBlogPostBySlugQuery(slug));
        return Ok(result);
    }
}
