using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Application.Features.Testimonials.Queries.GetTestimonials;
using ShahidPortfolio.Domain.Entities;

namespace ShahidPortfolio.Application.Features.Testimonials.Commands;

// 1. Public Submission Command
public record SubmitPublicTestimonialCommand(
    string ClientName,
    string Role,
    string Company,
    string? AvatarUrl,
    string Content,
    string? LinkedInUrl,
    int Rating,
    string? Relationship
) : IRequest<ApiResponse<bool>>;

public class SubmitPublicTestimonialValidator : AbstractValidator<SubmitPublicTestimonialCommand>
{
    public SubmitPublicTestimonialValidator()
    {
        RuleFor(x => x.ClientName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Role).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Company).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Content).NotEmpty().MinimumLength(10).MaximumLength(2000);
        RuleFor(x => x.Rating).InclusiveBetween(1, 5);
    }
}

public class SubmitPublicTestimonialCommandHandler : IRequestHandler<SubmitPublicTestimonialCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _context;

    public SubmitPublicTestimonialCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(SubmitPublicTestimonialCommand request, CancellationToken cancellationToken)
    {
        var testimonial = new Testimonial
        {
            ClientName = request.ClientName.Trim(),
            Role = request.Role.Trim(),
            Company = request.Company.Trim(),
            AvatarUrl = request.AvatarUrl?.Trim(),
            Content = request.Content.Trim(),
            LinkedInUrl = request.LinkedInUrl?.Trim(),
            Rating = request.Rating,
            Relationship = request.Relationship?.Trim() ?? "Colleague / Client",
            IsApproved = false, // Must be approved by Admin
            IsActive = true,
            SubmittedAtUtc = DateTime.UtcNow
        };

        await _context.Testimonials.AddAsync(testimonial, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Ok(true, "Your endorsement has been submitted successfully and is pending review.");
    }
}

// 2. Admin Get All Testimonials Query
public record GetAdminTestimonialsQuery : IRequest<ApiResponse<List<TestimonialDto>>>;

public class GetAdminTestimonialsQueryHandler : IRequestHandler<GetAdminTestimonialsQuery, ApiResponse<List<TestimonialDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetAdminTestimonialsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<TestimonialDto>>> Handle(GetAdminTestimonialsQuery request, CancellationToken cancellationToken)
    {
        var list = await _context.Testimonials
            .AsNoTracking()
            .OrderBy(t => t.IsApproved) // Unapproved first
            .ThenByDescending(t => t.SubmittedAtUtc)
            .Select(t => new TestimonialDto
            {
                Id = t.Id,
                ClientName = t.ClientName,
                Role = t.Role,
                Company = t.Company,
                AvatarUrl = t.AvatarUrl,
                Content = t.Content,
                LinkedInUrl = t.LinkedInUrl,
                Rating = t.Rating,
                Relationship = t.Relationship,
                IsApproved = t.IsApproved,
                IsActive = t.IsActive,
                DisplayOrder = t.DisplayOrder,
                SubmittedAtUtc = t.SubmittedAtUtc
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<TestimonialDto>>.Ok(list);
    }
}

// 3. Admin Approve / Reject Testimonial
public record ApproveTestimonialCommand(Guid Id) : IRequest<ApiResponse<bool>>;
public record RejectTestimonialCommand(Guid Id) : IRequest<ApiResponse<bool>>;

public class ApproveTestimonialCommandHandler : IRequestHandler<ApproveTestimonialCommand, ApiResponse<bool>>, IRequestHandler<RejectTestimonialCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _context;

    public ApproveTestimonialCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(ApproveTestimonialCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.Testimonials.FindAsync(new object[] { request.Id }, cancellationToken);
        if (item == null) return ApiResponse<bool>.Fail("Testimonial not found.");

        item.IsApproved = true;
        await _context.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.Ok(true, "Testimonial approved successfully.");
    }

    public async Task<ApiResponse<bool>> Handle(RejectTestimonialCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.Testimonials.FindAsync(new object[] { request.Id }, cancellationToken);
        if (item == null) return ApiResponse<bool>.Fail("Testimonial not found.");

        item.IsApproved = false;
        await _context.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.Ok(true, "Testimonial unapproved/rejected.");
    }
}

// 4. Admin Create / Update / Delete Commands
public record CreateAdminTestimonialCommand(
    string ClientName,
    string Role,
    string Company,
    string? AvatarUrl,
    string Content,
    string? LinkedInUrl,
    int Rating,
    string? Relationship,
    bool IsApproved,
    int DisplayOrder
) : IRequest<ApiResponse<Guid>>;

public record UpdateAdminTestimonialCommand(
    Guid Id,
    string ClientName,
    string Role,
    string Company,
    string? AvatarUrl,
    string Content,
    string? LinkedInUrl,
    int Rating,
    string? Relationship,
    bool IsApproved,
    int DisplayOrder
) : IRequest<ApiResponse<bool>>;

public record DeleteTestimonialCommand(Guid Id) : IRequest<ApiResponse<bool>>;

public class AdminTestimonialCrudHandler :
    IRequestHandler<CreateAdminTestimonialCommand, ApiResponse<Guid>>,
    IRequestHandler<UpdateAdminTestimonialCommand, ApiResponse<bool>>,
    IRequestHandler<DeleteTestimonialCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _context;

    public AdminTestimonialCrudHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(CreateAdminTestimonialCommand request, CancellationToken cancellationToken)
    {
        var entity = new Testimonial
        {
            ClientName = request.ClientName,
            Role = request.Role,
            Company = request.Company,
            AvatarUrl = request.AvatarUrl,
            Content = request.Content,
            LinkedInUrl = request.LinkedInUrl,
            Rating = request.Rating,
            Relationship = request.Relationship ?? "Client",
            IsApproved = request.IsApproved,
            DisplayOrder = request.DisplayOrder,
            SubmittedAtUtc = DateTime.UtcNow
        };

        await _context.Testimonials.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return ApiResponse<Guid>.Ok(entity.Id, "Testimonial created successfully.");
    }

    public async Task<ApiResponse<bool>> Handle(UpdateAdminTestimonialCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Testimonials.FindAsync(new object[] { request.Id }, cancellationToken);
        if (entity == null) return ApiResponse<bool>.Fail("Testimonial not found.");

        entity.ClientName = request.ClientName;
        entity.Role = request.Role;
        entity.Company = request.Company;
        entity.AvatarUrl = request.AvatarUrl;
        entity.Content = request.Content;
        entity.LinkedInUrl = request.LinkedInUrl;
        entity.Rating = request.Rating;
        entity.Relationship = request.Relationship;
        entity.IsApproved = request.IsApproved;
        entity.DisplayOrder = request.DisplayOrder;

        await _context.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.Ok(true, "Testimonial updated successfully.");
    }

    public async Task<ApiResponse<bool>> Handle(DeleteTestimonialCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Testimonials.FindAsync(new object[] { request.Id }, cancellationToken);
        if (entity == null) return ApiResponse<bool>.Fail("Testimonial not found.");

        _context.Testimonials.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.Ok(true, "Testimonial deleted successfully.");
    }
}
