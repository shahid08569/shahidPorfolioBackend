using FluentValidation;
using MediatR;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Domain.Entities;
using ShahidPortfolio.Domain.Enums;

namespace ShahidPortfolio.Application.Features.Experiences.Commands;

public record CreateExperienceCommand(
    string Company,
    string Role,
    string? Location,
    EmploymentType EmploymentType,
    DateOnly StartDate,
    DateOnly? EndDate,
    bool IsCurrent,
    string AchievementsJson,
    string TechStackJson,
    int DisplayOrder
) : IRequest<ApiResponse<Guid>>;

public class CreateExperienceCommandHandler : IRequestHandler<CreateExperienceCommand, ApiResponse<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateExperienceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(CreateExperienceCommand request, CancellationToken cancellationToken)
    {
        var exp = new Experience
        {
            Company = request.Company,
            Role = request.Role,
            Location = request.Location,
            EmploymentType = request.EmploymentType,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsCurrent = request.IsCurrent,
            AchievementsJson = string.IsNullOrWhiteSpace(request.AchievementsJson) ? "[]" : request.AchievementsJson,
            TechStackJson = string.IsNullOrWhiteSpace(request.TechStackJson) ? "[]" : request.TechStackJson,
            DisplayOrder = request.DisplayOrder
        };

        await _context.Experiences.AddAsync(exp, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.Ok(exp.Id, "Experience created successfully.");
    }
}

public record UpdateExperienceCommand(
    Guid Id,
    string Company,
    string Role,
    string? Location,
    EmploymentType EmploymentType,
    DateOnly StartDate,
    DateOnly? EndDate,
    bool IsCurrent,
    string AchievementsJson,
    string TechStackJson,
    int DisplayOrder
) : IRequest<ApiResponse<bool>>;

public class UpdateExperienceCommandHandler : IRequestHandler<UpdateExperienceCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _context;

    public UpdateExperienceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(UpdateExperienceCommand request, CancellationToken cancellationToken)
    {
        var exp = await _context.Experiences.FindAsync(new object[] { request.Id }, cancellationToken);
        if (exp == null)
        {
            return ApiResponse<bool>.Fail("Experience not found.");
        }

        exp.Company = request.Company;
        exp.Role = request.Role;
        exp.Location = request.Location;
        exp.EmploymentType = request.EmploymentType;
        exp.StartDate = request.StartDate;
        exp.EndDate = request.EndDate;
        exp.IsCurrent = request.IsCurrent;
        exp.AchievementsJson = string.IsNullOrWhiteSpace(request.AchievementsJson) ? "[]" : request.AchievementsJson;
        exp.TechStackJson = string.IsNullOrWhiteSpace(request.TechStackJson) ? "[]" : request.TechStackJson;
        exp.DisplayOrder = request.DisplayOrder;

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Ok(true, "Experience updated successfully.");
    }
}

public record DeleteExperienceCommand(Guid Id) : IRequest<ApiResponse<bool>>;

public class DeleteExperienceCommandHandler : IRequestHandler<DeleteExperienceCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _context;

    public DeleteExperienceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(DeleteExperienceCommand request, CancellationToken cancellationToken)
    {
        var exp = await _context.Experiences.FindAsync(new object[] { request.Id }, cancellationToken);
        if (exp == null)
        {
            return ApiResponse<bool>.Fail("Experience not found.");
        }

        _context.Experiences.Remove(exp);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Ok(true, "Experience deleted successfully.");
    }
}
