using MediatR;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Domain.Entities;

namespace ShahidPortfolio.Application.Features.Experiences.Commands;

public record CreateEducationCommand(
    string Institution,
    string Degree,
    string FieldOfStudy,
    DateOnly StartDate,
    DateOnly? EndDate,
    string? GradeOrHonors,
    int DisplayOrder
) : IRequest<ApiResponse<Guid>>;

public class CreateEducationCommandHandler : IRequestHandler<CreateEducationCommand, ApiResponse<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateEducationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(CreateEducationCommand request, CancellationToken cancellationToken)
    {
        var edu = new Education
        {
            Institution = request.Institution,
            Degree = request.Degree,
            FieldOfStudy = request.FieldOfStudy,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            GradeOrHonors = request.GradeOrHonors,
            DisplayOrder = request.DisplayOrder
        };

        await _context.Educations.AddAsync(edu, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.Ok(edu.Id, "Education created successfully.");
    }
}

public record UpdateEducationCommand(
    Guid Id,
    string Institution,
    string Degree,
    string FieldOfStudy,
    DateOnly StartDate,
    DateOnly? EndDate,
    string? GradeOrHonors,
    int DisplayOrder
) : IRequest<ApiResponse<bool>>;

public class UpdateEducationCommandHandler : IRequestHandler<UpdateEducationCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _context;

    public UpdateEducationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(UpdateEducationCommand request, CancellationToken cancellationToken)
    {
        var edu = await _context.Educations.FindAsync(new object[] { request.Id }, cancellationToken);
        if (edu == null)
        {
            return ApiResponse<bool>.Fail("Education not found.");
        }

        edu.Institution = request.Institution;
        edu.Degree = request.Degree;
        edu.FieldOfStudy = request.FieldOfStudy;
        edu.StartDate = request.StartDate;
        edu.EndDate = request.EndDate;
        edu.GradeOrHonors = request.GradeOrHonors;
        edu.DisplayOrder = request.DisplayOrder;

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Ok(true, "Education updated successfully.");
    }
}

public record DeleteEducationCommand(Guid Id) : IRequest<ApiResponse<bool>>;

public class DeleteEducationCommandHandler : IRequestHandler<DeleteEducationCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _context;

    public DeleteEducationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(DeleteEducationCommand request, CancellationToken cancellationToken)
    {
        var edu = await _context.Educations.FindAsync(new object[] { request.Id }, cancellationToken);
        if (edu == null)
        {
            return ApiResponse<bool>.Fail("Education not found.");
        }

        _context.Educations.Remove(edu);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Ok(true, "Education deleted successfully.");
    }
}
