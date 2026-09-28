using MediatR;
using Microsoft.EntityFrameworkCore;
using ShahidPortfolio.Application.Common.Exceptions;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Application.Common.Models;

namespace ShahidPortfolio.Application.Features.Projects.Queries.GetProjectBySlug;

public record GetProjectBySlugQuery(string Slug) : IRequest<ApiResponse<ProjectDetailDto>>;

public class GetProjectBySlugQueryHandler : IRequestHandler<GetProjectBySlugQuery, ApiResponse<ProjectDetailDto>>
{
    private readonly IApplicationDbContext _context;

    public GetProjectBySlugQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<ProjectDetailDto>> Handle(GetProjectBySlugQuery request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .AsNoTracking()
            .Where(p => p.Slug.ToLower() == request.Slug.ToLower())
            .Select(p => new ProjectDetailDto
            {
                Id = p.Id,
                Title = p.Title,
                Slug = p.Slug,
                Summary = p.Summary,
                ProblemStatement = p.ProblemStatement,
                SolutionStatement = p.SolutionStatement,
                ArchitectureOverview = p.ArchitectureOverview,
                KeyMetrics = p.KeyMetrics,
                LessonsLearned = p.LessonsLearned,
                ThumbnailUrl = p.ThumbnailUrl,
                BannerUrl = p.BannerUrl,
                LiveUrl = p.LiveUrl,
                GithubUrl = p.GithubUrl,
                TechStackJson = p.TechStackJson,
                IsCaseStudy = p.IsCaseStudy,
                CreatedAtUtc = p.CreatedAtUtc
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (project == null)
        {
            throw new NotFoundException("Project", request.Slug);
        }

        return ApiResponse<ProjectDetailDto>.Ok(project);
    }
}
