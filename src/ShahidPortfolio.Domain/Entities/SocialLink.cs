using ShahidPortfolio.Domain.Common;

namespace ShahidPortfolio.Domain.Entities;

public class SocialLink : BaseEntity
{
    public string Platform { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string IconKey { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
