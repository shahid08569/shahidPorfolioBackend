using ShahidPortfolio.Domain.Common;

namespace ShahidPortfolio.Domain.Entities;

public class Education : BaseEntity
{
    public string Institution { get; set; } = string.Empty;
    public string Degree { get; set; } = string.Empty;
    public string FieldOfStudy { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? GradeOrHonors { get; set; }
    public int DisplayOrder { get; set; }
}
