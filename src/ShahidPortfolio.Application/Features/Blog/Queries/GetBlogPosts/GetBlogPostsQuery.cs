using MediatR;
using Microsoft.EntityFrameworkCore;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Domain.Enums;

namespace ShahidPortfolio.Application.Features.Blog.Queries.GetBlogPosts;

public record GetBlogPostsQuery(string? Tag = null) : IRequest<ApiResponse<List<BlogPostCardDto>>>;

public class GetBlogPostsQueryHandler : IRequestHandler<GetBlogPostsQuery, ApiResponse<List<BlogPostCardDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetBlogPostsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<BlogPostCardDto>>> Handle(GetBlogPostsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.BlogPosts
            .AsNoTracking()
            .Where(b => b.Status == BlogStatus.Published);

        if (!string.IsNullOrWhiteSpace(request.Tag))
        {
            query = query.Where(b => b.TagsJson.Contains(request.Tag));
        }

        var posts = await query
            .OrderByDescending(b => b.PublishedAtUtc)
            .Select(b => new BlogPostCardDto
            {
                Id = b.Id,
                Title = b.Title,
                Slug = b.Slug,
                Summary = b.Summary,
                CoverImageUrl = b.CoverImageUrl,
                TagsJson = b.TagsJson,
                ReadTimeMinutes = b.ReadTimeMinutes,
                PublishedAtUtc = b.PublishedAtUtc
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<BlogPostCardDto>>.Ok(posts);
    }
}
