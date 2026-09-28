namespace ShahidPortfolio.Application.Features.Experiences.Queries.GetTimeline;

public class TimelineDto
{
    public List<ExperienceItemDto> Experiences { get; set; } = new();
    public List<EducationItemDto> Educations { get; set; } = new();
}

public class ExperienceItemDto
{
    public Guid Id { get; set; }
    public string Company { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? Location { get; set; }
    public string EmploymentType { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string? EndDate { get; set; }
    public bool IsCurrent { get; set; }
    public string AchievementsJson { get; set; } = "[]";
    public string TechStackJson { get; set; } = "[]";
    public int DisplayOrder { get; set; }
}

public class EducationItemDto
{
    public Guid Id { get; set; }
    public string Institution { get; set; } = string.Empty;
    public string Degree { get; set; } = string.Empty;
    public string FieldOfStudy { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string? EndDate { get; set; }
    public string? GradeOrHonors { get; set; }
    public int DisplayOrder { get; set; }
}
