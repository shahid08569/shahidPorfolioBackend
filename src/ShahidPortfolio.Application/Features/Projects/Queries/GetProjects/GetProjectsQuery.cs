using MediatR;
using Microsoft.EntityFrameworkCore;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Application.Common.Models;

namespace ShahidPortfolio.Application.Features.Projects.Queries.GetProjects;

public record GetProjectsQuery(bool? IsFeatured = null, string? Tag = null) : IRequest<ApiResponse<List<ProjectCardDto>>>;

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, ApiResponse<List<ProjectCardDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetProjectsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<ProjectCardDto>>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Projects.AsNoTracking();

        if (request.IsFeatured.HasValue)
        {
            query = query.Where(p => p.IsFeatured == request.IsFeatured.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Tag))
        {
            query = query.Where(p => p.TechStackJson.Contains(request.Tag));
        }

        var projects = await query
            .OrderBy(p => p.DisplayOrder)
            .ThenByDescending(p => p.CreatedAtUtc)
            .Select(p => new ProjectCardDto
            {
                Id = p.Id,
                Title = p.Title,
                Slug = p.Slug,
                Summary = p.Summary,
                ThumbnailUrl = p.ThumbnailUrl,
                LiveUrl = p.LiveUrl,
                GithubUrl = p.GithubUrl,
                TechStackJson = p.TechStackJson,
                IsFeatured = p.IsFeatured,
                IsCaseStudy = p.IsCaseStudy,
                DisplayOrder = p.DisplayOrder
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<ProjectCardDto>>.Ok(projects);
    }
}
