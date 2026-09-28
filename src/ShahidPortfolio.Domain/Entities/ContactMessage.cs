using ShahidPortfolio.Domain.Common;

namespace ShahidPortfolio.Domain.Entities;

public class ContactMessage : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
    public bool IsRead { get; set; }
    public DateTime ReceivedAtUtc { get; set; } = DateTime.UtcNow;
}
