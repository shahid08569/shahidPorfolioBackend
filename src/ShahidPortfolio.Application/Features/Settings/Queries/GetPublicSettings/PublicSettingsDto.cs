namespace ShahidPortfolio.Application.Features.Settings.Queries.GetPublicSettings;

public class PublicSettingsDto
{
    public string FullName { get; set; } = string.Empty;
    public string ProfessionalTitle { get; set; } = string.Empty;
    public string OneLineBio { get; set; } = string.Empty;
    public string AboutSummary { get; set; } = string.Empty;
    public string AvailabilityStatus { get; set; } = string.Empty;
    public string CurrentLocation { get; set; } = string.Empty;
    public string CvUrl { get; set; } = string.Empty;
    public string WhatsAppNumber { get; set; } = string.Empty;
    public string HeroCodeTitle { get; set; } = string.Empty;
    public string HeroCodeSnippet { get; set; } = string.Empty;
    public string HeroBadgesJson { get; set; } = string.Empty;
    public List<SocialLinkDto> SocialLinks { get; set; } = new();
}

public class SocialLinkDto
{
    public string Platform { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string IconKey { get; set; } = string.Empty;
}
