using MediatR;
using Microsoft.EntityFrameworkCore;
using ShahidPortfolio.Application.Common.Exceptions;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Domain.Enums;

namespace ShahidPortfolio.Application.Features.Blog.Queries.GetBlogPostBySlug;

public record GetBlogPostBySlugQuery(string Slug) : IRequest<ApiResponse<BlogPostDetailDto>>;

public class GetBlogPostBySlugQueryHandler : IRequestHandler<GetBlogPostBySlugQuery, ApiResponse<BlogPostDetailDto>>
{
    private readonly IApplicationDbContext _context;

    public GetBlogPostBySlugQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<BlogPostDetailDto>> Handle(GetBlogPostBySlugQuery request, CancellationToken cancellationToken)
    {
        var post = await _context.BlogPosts
            .AsNoTracking()
            .Where(b => b.Slug.ToLower() == request.Slug.ToLower() && b.Status == BlogStatus.Published)
            .Select(b => new BlogPostDetailDto
            {
                Id = b.Id,
                Title = b.Title,
                Slug = b.Slug,
                Summary = b.Summary,
                ContentMarkdown = b.ContentMarkdown,
                CoverImageUrl = b.CoverImageUrl,
                TagsJson = b.TagsJson,
                ReadTimeMinutes = b.ReadTimeMinutes,
                PublishedAtUtc = b.PublishedAtUtc
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (post == null)
        {
            throw new NotFoundException("BlogPost", request.Slug);
        }

        return ApiResponse<BlogPostDetailDto>.Ok(post);
    }
}
