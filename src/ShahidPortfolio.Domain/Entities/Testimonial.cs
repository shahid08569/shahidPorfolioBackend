using ShahidPortfolio.Domain.Common;

namespace ShahidPortfolio.Domain.Entities;

public class Testimonial : BaseEntity
{
    public string ClientName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? LinkedInUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
}
