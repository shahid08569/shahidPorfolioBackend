using ShahidPortfolio.Domain.Enums;

namespace ShahidPortfolio.Application.Features.Skills.Queries.GetSkills;

public class SkillCategoryGroupDto
{
    public SkillCategory Category { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public List<SkillItemDto> Skills { get; set; } = new();
}

public class SkillItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? IconKey { get; set; }
    public int Proficiency { get; set; }
    public bool IsTopSkill { get; set; }
    public int DisplayOrder { get; set; }
}
