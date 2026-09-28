using MediatR;
using Microsoft.EntityFrameworkCore;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Application.Common.Models;

namespace ShahidPortfolio.Application.Features.Experiences.Queries.GetTimeline;

public record GetTimelineQuery : IRequest<ApiResponse<TimelineDto>>;

public class GetTimelineQueryHandler : IRequestHandler<GetTimelineQuery, ApiResponse<TimelineDto>>
{
    private readonly IApplicationDbContext _context;

    public GetTimelineQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<TimelineDto>> Handle(GetTimelineQuery request, CancellationToken cancellationToken)
    {
        var experiences = await _context.Experiences
            .AsNoTracking()
            .OrderBy(e => e.DisplayOrder)
            .ThenByDescending(e => e.StartDate)
            .Select(e => new ExperienceItemDto
            {
                Id = e.Id,
                Company = e.Company,
                Role = e.Role,
                Location = e.Location,
                EmploymentType = e.EmploymentType.ToString(),
                StartDate = e.StartDate.ToString("MMM yyyy"),
                EndDate = e.IsCurrent ? "Present" : (e.EndDate.HasValue ? e.EndDate.Value.ToString("MMM yyyy") : null),
                IsCurrent = e.IsCurrent,
                AchievementsJson = e.AchievementsJson,
                TechStackJson = e.TechStackJson,
                DisplayOrder = e.DisplayOrder
            })
            .ToListAsync(cancellationToken);

        var educations = await _context.Educations
            .AsNoTracking()
            .OrderBy(ed => ed.DisplayOrder)
            .ThenByDescending(ed => ed.StartDate)
            .Select(ed => new EducationItemDto
            {
                Id = ed.Id,
                Institution = ed.Institution,
                Degree = ed.Degree,
                FieldOfStudy = ed.FieldOfStudy,
                StartDate = ed.StartDate.ToString("yyyy"),
                EndDate = ed.EndDate.HasValue ? ed.EndDate.Value.ToString("yyyy") : "Present",
                GradeOrHonors = ed.GradeOrHonors,
                DisplayOrder = ed.DisplayOrder
            })
            .ToListAsync(cancellationToken);

        var dto = new TimelineDto
        {
            Experiences = experiences,
            Educations = educations
        };

        return ApiResponse<TimelineDto>.Ok(dto);
    }
}
