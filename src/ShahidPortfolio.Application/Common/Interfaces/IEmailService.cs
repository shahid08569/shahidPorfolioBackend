namespace ShahidPortfolio.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendContactNotificationAsync(string senderName, string senderEmail, string subject, string message, CancellationToken cancellationToken = default);
}
