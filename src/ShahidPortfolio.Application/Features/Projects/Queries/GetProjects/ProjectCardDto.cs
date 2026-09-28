namespace ShahidPortfolio.Application.Features.Projects.Queries.GetProjects;

public class ProjectCardDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string? LiveUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string TechStackJson { get; set; } = "[]";
    public bool IsFeatured { get; set; }
    public bool IsCaseStudy { get; set; }
    public int DisplayOrder { get; set; }
}
