using MediatR;
using Microsoft.EntityFrameworkCore;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Application.Common.Models;

namespace ShahidPortfolio.Application.Features.Settings.Queries.GetPublicSettings;

public record GetPublicSettingsQuery : IRequest<ApiResponse<PublicSettingsDto>>;

public class GetPublicSettingsQueryHandler : IRequestHandler<GetPublicSettingsQuery, ApiResponse<PublicSettingsDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPublicSettingsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<PublicSettingsDto>> Handle(GetPublicSettingsQuery request, CancellationToken cancellationToken)
    {
        var settings = await _context.SiteSettings
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        var socials = await _context.SocialLinks
            .AsNoTracking()
            .Where(s => s.IsActive)
            .OrderBy(s => s.DisplayOrder)
            .Select(s => new SocialLinkDto
            {
                Platform = s.Platform,
                Url = s.Url,
                IconKey = s.IconKey
            })
            .ToListAsync(cancellationToken);

        var dto = new PublicSettingsDto
        {
            FullName = settings?.FullName ?? "Shahid Hussain",
            ProfessionalTitle = settings?.ProfessionalTitle ?? "Full-Stack .NET & Angular Developer",
            OneLineBio = settings?.OneLineBio ?? "Architecting robust enterprise solutions with .NET Clean Architecture and Angular.",
            AboutSummary = settings?.AboutSummary ?? string.Empty,
            AvailabilityStatus = settings?.AvailabilityStatus ?? "Open to Work",
            CurrentLocation = settings?.CurrentLocation ?? "Pakistan",
            CvUrl = settings?.CvUrl ?? "/uploads/Shahid_Hussain_CV.pdf",
            WhatsAppNumber = settings?.WhatsAppNumber ?? "923000000000",
            HeroCodeTitle = settings?.HeroCodeTitle ?? "ShahidPortfolio.sln - Clean Architecture",
            HeroCodeSnippet = settings?.HeroCodeSnippet ?? @"public class SolutionArchitect
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
}",
            HeroBadgesJson = settings?.HeroBadgesJson ?? @"[""Clean Architecture"", ""CQRS / MediatR"", ""Angular 22 Signals""]",
            SocialLinks = socials
        };

        return ApiResponse<PublicSettingsDto>.Ok(dto);
    }
}
