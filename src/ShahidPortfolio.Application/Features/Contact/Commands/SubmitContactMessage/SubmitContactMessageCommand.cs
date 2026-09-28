using FluentValidation;
using MediatR;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Domain.Entities;

namespace ShahidPortfolio.Application.Features.Contact.Commands.SubmitContactMessage;

public record SubmitContactMessageCommand(
    string Name,
    string Email,
    string Subject,
    string Message,
    string? Honeypot = null,
    string? IpAddress = null
) : IRequest<ApiResponse<bool>>;

public class SubmitContactMessageCommandValidator : AbstractValidator<SubmitContactMessageCommand>
{
    public SubmitContactMessageCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(150).WithMessage("Email must not exceed 150 characters.");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Subject is required.")
            .MaximumLength(200).WithMessage("Subject must not exceed 200 characters.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required.")
            .MinimumLength(10).WithMessage("Message must be at least 10 characters long.")
            .MaximumLength(2500).WithMessage("Message must not exceed 2500 characters.");
    }
}

public class SubmitContactMessageCommandHandler : IRequestHandler<SubmitContactMessageCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public SubmitContactMessageCommandHandler(IApplicationDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task<ApiResponse<bool>> Handle(SubmitContactMessageCommand request, CancellationToken cancellationToken)
    {
        // 1. Anti-spam honeypot defense: If the hidden honeypot field is filled by a bot, silently return OK
        if (!string.IsNullOrWhiteSpace(request.Honeypot))
        {
            return ApiResponse<bool>.Ok(true, "Message received successfully.");
        }

        // 2. Persist message to database
        var message = new ContactMessage
        {
            Name = request.Name.Trim(),
            Email = request.Email.Trim().ToLowerInvariant(),
            Subject = request.Subject.Trim(),
            Message = request.Message.Trim(),
            IpAddress = request.IpAddress,
            IsRead = false,
            ReceivedAtUtc = DateTime.UtcNow
        };

        await _context.ContactMessages.AddAsync(message, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // 3. Trigger notification email asynchronously
        _ = _emailService.SendContactNotificationAsync(
            message.Name,
            message.Email,
            message.Subject,
            message.Message,
            cancellationToken);

        return ApiResponse<bool>.Ok(true, "Thank you! Your message has been sent successfully.");
    }
}
