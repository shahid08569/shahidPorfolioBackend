using MediatR;
using Microsoft.EntityFrameworkCore;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Application.Common.Models;

namespace ShahidPortfolio.Application.Features.Certificates.Queries;

public record GetCertificatesQuery(bool IncludeInactive = false) : IRequest<ApiResponse<List<CertificateDto>>>;

public class GetCertificatesQueryHandler : IRequestHandler<GetCertificatesQuery, ApiResponse<List<CertificateDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetCertificatesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<CertificateDto>>> Handle(GetCertificatesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Certificates.AsNoTracking();
        if (!request.IncludeInactive)
        {
            query = query.Where(c => c.IsActive);
        }

        var certificates = await query
            .OrderBy(c => c.DisplayOrder)
            .ThenByDescending(c => c.IssueDate)
            .Select(c => new CertificateDto
            {
                Id = c.Id,
                Title = c.Title,
                IssuingOrganization = c.IssuingOrganization,
                IssueDate = c.IssueDate,
                ExpirationDate = c.ExpirationDate,
                CredentialId = c.CredentialId,
                CredentialUrl = c.CredentialUrl,
                ImageUrl = c.ImageUrl,
                DisplayOrder = c.DisplayOrder,
                IsActive = c.IsActive
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<CertificateDto>>.Ok(certificates);
    }
}
