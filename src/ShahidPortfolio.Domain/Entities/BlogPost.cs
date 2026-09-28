using ShahidPortfolio.Domain.Common;
using ShahidPortfolio.Domain.Enums;

namespace ShahidPortfolio.Domain.Entities;

public class BlogPost : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string ContentMarkdown { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string TagsJson { get; set; } = "[]";
    public BlogStatus Status { get; set; } = BlogStatus.Draft;
    public int ReadTimeMinutes { get; set; } = 5;
    public DateTime? PublishedAtUtc { get; set; }
}
