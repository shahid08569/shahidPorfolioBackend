using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Domain.Entities;

namespace ShahidPortfolio.Application.Features.Projects.Commands.CreateProject;

public record CreateProjectCommand(
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
) : IRequest<ApiResponse<Guid>>;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Slug).NotEmpty().MaximumLength(160);
        RuleFor(x => x.Summary).NotEmpty().MaximumLength(500);
    }
}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ApiResponse<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateProjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var slugExists = await _context.Projects
            .AnyAsync(p => p.Slug.ToLower() == request.Slug.ToLower(), cancellationToken);

        if (slugExists)
        {
            return ApiResponse<Guid>.Fail("A project with this slug already exists.");
        }

        var project = new Project
        {
            Title = request.Title,
            Slug = request.Slug.Trim().ToLowerInvariant(),
            Summary = request.Summary,
            ProblemStatement = request.ProblemStatement,
            SolutionStatement = request.SolutionStatement,
            ArchitectureOverview = request.ArchitectureOverview,
            KeyMetrics = request.KeyMetrics,
            LessonsLearned = request.LessonsLearned,
            ThumbnailUrl = request.ThumbnailUrl,
            BannerUrl = request.BannerUrl,
            LiveUrl = request.LiveUrl,
            GithubUrl = request.GithubUrl,
            TechStackJson = string.IsNullOrWhiteSpace(request.TechStackJson) ? "[]" : request.TechStackJson,
            IsFeatured = request.IsFeatured,
            IsCaseStudy = request.IsCaseStudy,
            DisplayOrder = request.DisplayOrder,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _context.Projects.AddAsync(project, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.Ok(project.Id, "Project created successfully.");
    }
}
