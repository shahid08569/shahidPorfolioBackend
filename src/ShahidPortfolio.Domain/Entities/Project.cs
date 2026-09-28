using ShahidPortfolio.Domain.Common;
using ShahidPortfolio.Domain.Enums;

namespace ShahidPortfolio.Domain.Entities;

public class Project : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string? ProblemStatement { get; set; }
    public string? SolutionStatement { get; set; }
    public string? ArchitectureOverview { get; set; }
    public string? KeyMetrics { get; set; }
    public string? LessonsLearned { get; set; }
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string? BannerUrl { get; set; }
    public string? LiveUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string TechStackJson { get; set; } = "[]";
    public ProjectStatus Status { get; set; } = ProjectStatus.Completed;
    public bool IsFeatured { get; set; }
    public bool IsCaseStudy { get; set; }
    public int DisplayOrder { get; set; }
}
