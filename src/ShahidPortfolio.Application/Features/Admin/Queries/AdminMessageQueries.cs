using MediatR;
using Microsoft.EntityFrameworkCore;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Application.Common.Models;

namespace ShahidPortfolio.Application.Features.Admin.Queries;

public record GetContactMessagesQuery : IRequest<ApiResponse<List<ContactMessageDto>>>;

public class ContactMessageDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime ReceivedAtUtc { get; set; }
}

public class GetContactMessagesQueryHandler : IRequestHandler<GetContactMessagesQuery, ApiResponse<List<ContactMessageDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetContactMessagesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<ContactMessageDto>>> Handle(GetContactMessagesQuery request, CancellationToken cancellationToken)
    {
        var messages = await _context.ContactMessages
            .AsNoTracking()
            .OrderByDescending(m => m.ReceivedAtUtc)
            .Select(m => new ContactMessageDto
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Subject = m.Subject,
                Message = m.Message,
                IsRead = m.IsRead,
                ReceivedAtUtc = m.ReceivedAtUtc
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<ContactMessageDto>>.Ok(messages);
    }
}

public record MarkContactMessageAsReadCommand(Guid Id) : IRequest<ApiResponse<bool>>;

public class MarkContactMessageAsReadCommandHandler : IRequestHandler<MarkContactMessageAsReadCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _context;

    public MarkContactMessageAsReadCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(MarkContactMessageAsReadCommand request, CancellationToken cancellationToken)
    {
        var msg = await _context.ContactMessages.FindAsync(new object[] { request.Id }, cancellationToken);
        if (msg == null)
        {
            return ApiResponse<bool>.Fail("Message not found.");
        }

        msg.IsRead = true;
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Ok(true, "Message marked as read.");
    }
}

public record DeleteContactMessageCommand(Guid Id) : IRequest<ApiResponse<bool>>;

public class DeleteContactMessageCommandHandler : IRequestHandler<DeleteContactMessageCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _context;

    public DeleteContactMessageCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(DeleteContactMessageCommand request, CancellationToken cancellationToken)
    {
        var msg = await _context.ContactMessages.FindAsync(new object[] { request.Id }, cancellationToken);
        if (msg == null)
        {
            return ApiResponse<bool>.Fail("Message not found.");
        }

        _context.ContactMessages.Remove(msg);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Ok(true, "Message deleted successfully.");
    }
}
