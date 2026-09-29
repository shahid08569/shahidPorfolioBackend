using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Domain.Entities;

namespace ShahidPortfolio.Application.Features.Certificates.Commands;

public record CreateCertificateCommand(
    string Title,
    string IssuingOrganization,
    DateTime IssueDate,
    string? ExpirationDate,
    string? CredentialId,
    string? CredentialUrl,
    string? ImageUrl,
    int DisplayOrder,
    bool IsActive
) : IRequest<ApiResponse<Guid>>;

public class CreateCertificateValidator : AbstractValidator<CreateCertificateCommand>
{
    public CreateCertificateValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
        RuleFor(x => x.IssuingOrganization).NotEmpty().MaximumLength(150);
    }
}

public record UpdateCertificateCommand(
    Guid Id,
    string Title,
    string IssuingOrganization,
    DateTime IssueDate,
    string? ExpirationDate,
    string? CredentialId,
    string? CredentialUrl,
    string? ImageUrl,
    int DisplayOrder,
    bool IsActive
) : IRequest<ApiResponse<bool>>;

public record DeleteCertificateCommand(Guid Id) : IRequest<ApiResponse<bool>>;

public class CertificateCommandHandler :
    IRequestHandler<CreateCertificateCommand, ApiResponse<Guid>>,
    IRequestHandler<UpdateCertificateCommand, ApiResponse<bool>>,
    IRequestHandler<DeleteCertificateCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _context;

    public CertificateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(CreateCertificateCommand request, CancellationToken cancellationToken)
    {
        var entity = new Certificate
        {
            Title = request.Title.Trim(),
            IssuingOrganization = request.IssuingOrganization.Trim(),
            IssueDate = request.IssueDate,
            ExpirationDate = request.ExpirationDate?.Trim(),
            CredentialId = request.CredentialId?.Trim(),
            CredentialUrl = request.CredentialUrl?.Trim(),
            ImageUrl = request.ImageUrl?.Trim(),
            DisplayOrder = request.DisplayOrder,
            IsActive = request.IsActive
        };

        await _context.Certificates.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.Ok(entity.Id, "Certificate created successfully.");
    }

    public async Task<ApiResponse<bool>> Handle(UpdateCertificateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Certificates.FindAsync(new object[] { request.Id }, cancellationToken);
        if (entity == null) return ApiResponse<bool>.Fail("Certificate not found.");

        entity.Title = request.Title.Trim();
        entity.IssuingOrganization = request.IssuingOrganization.Trim();
        entity.IssueDate = request.IssueDate;
        entity.ExpirationDate = request.ExpirationDate?.Trim();
        entity.CredentialId = request.CredentialId?.Trim();
        entity.CredentialUrl = request.CredentialUrl?.Trim();
        entity.ImageUrl = request.ImageUrl?.Trim();
        entity.DisplayOrder = request.DisplayOrder;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.Ok(true, "Certificate updated successfully.");
    }

    public async Task<ApiResponse<bool>> Handle(DeleteCertificateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Certificates.FindAsync(new object[] { request.Id }, cancellationToken);
        if (entity == null) return ApiResponse<bool>.Fail("Certificate not found.");

        _context.Certificates.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.Ok(true, "Certificate deleted successfully.");
    }
}
