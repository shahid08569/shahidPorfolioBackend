using ShahidPortfolio.Domain.Common;

namespace ShahidPortfolio.Domain.Entities;

public class SiteSettings : BaseEntity
{
    public string FullName { get; set; } = "Shahid Hussain";
    public string ProfessionalTitle { get; set; } = "Full-Stack .NET & Angular Developer";
    public string OneLineBio { get; set; } = "Building high-performance enterprise applications with .NET Clean Architecture and modern Angular.";
    public string AboutSummary { get; set; } = string.Empty;
    public string AvailabilityStatus { get; set; } = "Available for Full-time Roles";
    public string CurrentLocation { get; set; } = "Pakistan";
    public string CvUrl { get; set; } = "/uploads/Shahid_Hussain_CV.pdf";
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
}
