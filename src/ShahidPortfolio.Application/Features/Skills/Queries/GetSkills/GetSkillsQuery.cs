using MediatR;
using Microsoft.EntityFrameworkCore;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Domain.Enums;

namespace ShahidPortfolio.Application.Features.Skills.Queries.GetSkills;

public record GetSkillsQuery(bool? TopSkillsOnly = null) : IRequest<ApiResponse<List<SkillCategoryGroupDto>>>;

public class GetSkillsQueryHandler : IRequestHandler<GetSkillsQuery, ApiResponse<List<SkillCategoryGroupDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetSkillsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<SkillCategoryGroupDto>>> Handle(GetSkillsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Skills.AsNoTracking();

        if (request.TopSkillsOnly == true)
        {
            query = query.Where(s => s.IsTopSkill);
        }

        var skills = await query
            .OrderBy(s => s.Category)
            .ThenBy(s => s.DisplayOrder)
            .ToListAsync(cancellationToken);

        var grouped = Enum.GetValues<SkillCategory>()
            .Select(cat => new SkillCategoryGroupDto
            {
                Category = cat,
                CategoryName = cat switch
                {
                    SkillCategory.Frontend => "Frontend Development",
                    SkillCategory.Backend => "Backend & Architecture",
                    SkillCategory.Database => "Databases & Storage",
                    SkillCategory.ToolsDevOps => "Tools, DevOps & Cloud",
                    _ => cat.ToString()
                },
                Skills = skills
                    .Where(s => s.Category == cat)
                    .Select(s => new SkillItemDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        IconKey = s.IconKey,
                        Proficiency = s.Proficiency,
                        IsTopSkill = s.IsTopSkill,
                        DisplayOrder = s.DisplayOrder
                    })
                    .ToList()
            })
            .Where(g => g.Skills.Any())
            .ToList();

        return ApiResponse<List<SkillCategoryGroupDto>>.Ok(grouped);
    }
}
