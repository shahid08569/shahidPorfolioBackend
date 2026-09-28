namespace ShahidPortfolio.Application.Features.Blog.Queries.GetBlogPostBySlug;

public class BlogPostDetailDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string ContentMarkdown { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string TagsJson { get; set; } = "[]";
    public int ReadTimeMinutes { get; set; }
    public DateTime? PublishedAtUtc { get; set; }
}
