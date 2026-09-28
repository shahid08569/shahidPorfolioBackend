using FluentValidation;
using MediatR;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Domain.Entities;
using ShahidPortfolio.Domain.Enums;

namespace ShahidPortfolio.Application.Features.Skills.Commands;

public record CreateSkillCommand(
    string Name,
    SkillCategory Category,
    string? IconKey,
    int Proficiency,
    bool IsTopSkill,
    int DisplayOrder
) : IRequest<ApiResponse<Guid>>;

public class CreateSkillCommandValidator : AbstractValidator<CreateSkillCommand>
{
    public CreateSkillCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Proficiency).InclusiveBetween(1, 100);
    }
}

public class CreateSkillCommandHandler : IRequestHandler<CreateSkillCommand, ApiResponse<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateSkillCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
    {
        var skill = new Skill
        {
            Name = request.Name,
            Category = request.Category,
            IconKey = request.IconKey,
            Proficiency = request.Proficiency,
            IsTopSkill = request.IsTopSkill,
            DisplayOrder = request.DisplayOrder
        };

        await _context.Skills.AddAsync(skill, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.Ok(skill.Id, "Skill created successfully.");
    }
}

public record UpdateSkillCommand(
    Guid Id,
    string Name,
    SkillCategory Category,
    string? IconKey,
    int Proficiency,
    bool IsTopSkill,
    int DisplayOrder
) : IRequest<ApiResponse<bool>>;

public class UpdateSkillCommandValidator : AbstractValidator<UpdateSkillCommand>
{
    public UpdateSkillCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Proficiency).InclusiveBetween(1, 100);
    }
}

public class UpdateSkillCommandHandler : IRequestHandler<UpdateSkillCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _context;

    public UpdateSkillCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(UpdateSkillCommand request, CancellationToken cancellationToken)
    {
        var skill = await _context.Skills.FindAsync(new object[] { request.Id }, cancellationToken);
        if (skill == null)
        {
            return ApiResponse<bool>.Fail("Skill not found.");
        }

        skill.Name = request.Name;
        skill.Category = request.Category;
        skill.IconKey = request.IconKey;
        skill.Proficiency = request.Proficiency;
        skill.IsTopSkill = request.IsTopSkill;
        skill.DisplayOrder = request.DisplayOrder;

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Ok(true, "Skill updated successfully.");
    }
}

public record DeleteSkillCommand(Guid Id) : IRequest<ApiResponse<bool>>;

public class DeleteSkillCommandHandler : IRequestHandler<DeleteSkillCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _context;

    public DeleteSkillCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
    {
        var skill = await _context.Skills.FindAsync(new object[] { request.Id }, cancellationToken);
        if (skill == null)
        {
            return ApiResponse<bool>.Fail("Skill not found.");
        }

        _context.Skills.Remove(skill);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Ok(true, "Skill deleted successfully.");
    }
}
