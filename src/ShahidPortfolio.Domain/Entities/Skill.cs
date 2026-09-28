using ShahidPortfolio.Domain.Common;
using ShahidPortfolio.Domain.Enums;

namespace ShahidPortfolio.Domain.Entities;

public class Skill : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public SkillCategory Category { get; set; }
    public string? IconKey { get; set; }
    public int Proficiency { get; set; } = 90;
    public bool IsTopSkill { get; set; }
    public int DisplayOrder { get; set; }
}
