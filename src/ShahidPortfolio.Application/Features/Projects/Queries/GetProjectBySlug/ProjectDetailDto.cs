namespace ShahidPortfolio.Application.Features.Projects.Queries.GetProjectBySlug;

public class ProjectDetailDto
{
    public Guid Id { get; set; }
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
    public bool IsCaseStudy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
