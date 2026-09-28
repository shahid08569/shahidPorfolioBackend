using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Application.Common.Models;

namespace ShahidPortfolio.Application.Features.Projects.Commands.UpdateProject;

public record UpdateProjectCommand(
    Guid Id,
    string Title,
    string Slug,
    string Summary,
    string? ProblemStatement,
    string? SolutionStatement,
    string? ArchitectureOverview,
    string? KeyMetrics,
    string? LessonsLearned,
    string ThumbnailUrl,
    string? BannerUrl,
    string? LiveUrl,
    string? GithubUrl,
    string TechStackJson,
    bool IsFeatured,
    bool IsCaseStudy,
    int DisplayOrder
) : IRequest<ApiResponse<bool>>;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Slug).NotEmpty().MaximumLength(160);
        RuleFor(x => x.Summary).NotEmpty().MaximumLength(500);
    }
}

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _context;

    public UpdateProjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects.FindAsync(new object[] { request.Id }, cancellationToken);
        if (project == null)
        {
            return ApiResponse<bool>.Fail("Project not found.");
        }

        var slugExists = await _context.Projects
            .AnyAsync(p => p.Id != request.Id && p.Slug.ToLower() == request.Slug.ToLower(), cancellationToken);

        if (slugExists)
        {
            return ApiResponse<bool>.Fail("Another project with this slug already exists.");
        }

        project.Title = request.Title;
        project.Slug = request.Slug.Trim().ToLowerInvariant();
        project.Summary = request.Summary;
        project.ProblemStatement = request.ProblemStatement;
        project.SolutionStatement = request.SolutionStatement;
        project.ArchitectureOverview = request.ArchitectureOverview;
        project.KeyMetrics = request.KeyMetrics;
        project.LessonsLearned = request.LessonsLearned;
        project.ThumbnailUrl = request.ThumbnailUrl;
        project.BannerUrl = request.BannerUrl;
        project.LiveUrl = request.LiveUrl;
        project.GithubUrl = request.GithubUrl;
        project.TechStackJson = string.IsNullOrWhiteSpace(request.TechStackJson) ? "[]" : request.TechStackJson;
        project.IsFeatured = request.IsFeatured;
        project.IsCaseStudy = request.IsCaseStudy;
        project.DisplayOrder = request.DisplayOrder;
        project.UpdatedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Ok(true, "Project updated successfully.");
    }
}
