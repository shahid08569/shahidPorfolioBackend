using MediatR;
using Microsoft.EntityFrameworkCore;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Application.Common.Models;

namespace ShahidPortfolio.Application.Features.Testimonials.Queries.GetTestimonials;

public record GetTestimonialsQuery : IRequest<ApiResponse<List<TestimonialDto>>>;

public class GetTestimonialsQueryHandler : IRequestHandler<GetTestimonialsQuery, ApiResponse<List<TestimonialDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetTestimonialsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<TestimonialDto>>> Handle(GetTestimonialsQuery request, CancellationToken cancellationToken)
    {
        var testimonials = await _context.Testimonials
            .AsNoTracking()
            .Where(t => t.IsActive && t.IsApproved)
            .OrderBy(t => t.DisplayOrder)
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

        return ApiResponse<List<TestimonialDto>>.Ok(testimonials);
    }
}
