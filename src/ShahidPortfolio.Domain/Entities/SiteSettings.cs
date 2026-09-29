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
    public string WhatsAppNumber { get; set; } = "923000000000";
    public string HeroCodeTitle { get; set; } = "ShahidPortfolio.sln - Clean Architecture";
    public string HeroCodeSnippet { get; set; } = @"public class SolutionArchitect
{
    public string Name => ""Shahid Hussain"";
    public string[] CoreStack => new[]
    {
        "".NET 10 / C#"",
        ""ASP.NET Core Web API"",
        ""Clean Architecture & CQRS"",
        ""Angular 22 (SSR)"",
        ""SQL Server & EF Core""
    };
    public bool DeliverCleanCode() => true;
}";
    public string HeroBadgesJson { get; set; } = @"[""Clean Architecture"", ""CQRS / MediatR"", ""Angular 22 Signals""]";
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
}
