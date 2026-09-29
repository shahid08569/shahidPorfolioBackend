using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Domain.Entities;

namespace ShahidPortfolio.Application.Features.Admin.Commands;

public record UpdateSiteSettingsCommand(
    string FullName,
    string ProfessionalTitle,
    string OneLineBio,
    string AboutSummary,
    string AvailabilityStatus,
    string CurrentLocation,
    string CvUrl,
    string WhatsAppNumber,
    string HeroCodeTitle,
    string HeroCodeSnippet,
    string HeroBadgesJson,
    List<SocialLinkInputDto>? SocialLinks = null
) : IRequest<ApiResponse<bool>>;

public class SocialLinkInputDto
{
    public string Platform { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string IconKey { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}

public class UpdateSiteSettingsCommandHandler : IRequestHandler<UpdateSiteSettingsCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _context;

    public UpdateSiteSettingsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(UpdateSiteSettingsCommand request, CancellationToken cancellationToken)
    {
        var settings = await _context.SiteSettings.FirstOrDefaultAsync(cancellationToken);
        if (settings == null)
        {
            settings = new SiteSettings();
            await _context.SiteSettings.AddAsync(settings, cancellationToken);
        }

        settings.FullName = request.FullName;
        settings.ProfessionalTitle = request.ProfessionalTitle;
        settings.OneLineBio = request.OneLineBio;
        settings.AboutSummary = request.AboutSummary;
        settings.AvailabilityStatus = request.AvailabilityStatus;
        settings.CurrentLocation = request.CurrentLocation;
        settings.CvUrl = request.CvUrl;
        settings.WhatsAppNumber = string.IsNullOrWhiteSpace(request.WhatsAppNumber) ? "923000000000" : request.WhatsAppNumber;
        settings.HeroCodeTitle = string.IsNullOrWhiteSpace(request.HeroCodeTitle) ? "ShahidPortfolio.sln - Clean Architecture" : request.HeroCodeTitle;
        settings.HeroCodeSnippet = request.HeroCodeSnippet ?? string.Empty;
        settings.HeroBadgesJson = string.IsNullOrWhiteSpace(request.HeroBadgesJson) ? "[]" : request.HeroBadgesJson;

        // Update social links if provided
        if (request.SocialLinks != null)
        {
            var existingSocials = await _context.SocialLinks.ToListAsync(cancellationToken);
            _context.SocialLinks.RemoveRange(existingSocials);

            if (request.SocialLinks.Count > 0)
            {
                var newSocials = request.SocialLinks.Select(s => new SocialLink
                {
                    Platform = s.Platform ?? string.Empty,
                    Url = s.Url ?? string.Empty,
                    IconKey = s.IconKey ?? string.Empty,
                    DisplayOrder = s.DisplayOrder,
                    IsActive = true
                });
                await _context.SocialLinks.AddRangeAsync(newSocials, cancellationToken);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Ok(true, "Site settings updated successfully.");
    }
}
