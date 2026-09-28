using ShahidPortfolio.Domain.Common;
using ShahidPortfolio.Domain.Enums;

namespace ShahidPortfolio.Domain.Entities;

public class Experience : BaseEntity
{
    public string Company { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? Location { get; set; }
    public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public bool IsCurrent { get; set; }
    public string AchievementsJson { get; set; } = "[]";
    public string TechStackJson { get; set; } = "[]";
    public int DisplayOrder { get; set; }
}
