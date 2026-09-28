using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using ShahidPortfolio.Application.Common.Interfaces;

namespace ShahidPortfolio.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendContactNotificationAsync(string senderName, string senderEmail, string subject, string message, CancellationToken cancellationToken = default)
    {
        var smtpHost = _configuration["EmailSettings:SmtpHost"];
        var smtpPortString = _configuration["EmailSettings:SmtpPort"];
        var smtpUser = _configuration["EmailSettings:SmtpUser"];
        var smtpPass = _configuration["EmailSettings:SmtpPassword"];
        var receiverEmail = _configuration["EmailSettings:ReceiverEmail"] ?? "shahidhussaain08569@gmail.com";

        // If SMTP credentials are not configured yet, log the notification cleanly
        if (string.IsNullOrWhiteSpace(smtpHost) || string.IsNullOrWhiteSpace(smtpUser) || string.IsNullOrWhiteSpace(smtpPass))
        {
            _logger.LogInformation("--- NEW INBOUND CONTACT FORM MESSAGE ---");
            _logger.LogInformation("From: {Name} ({Email})", senderName, senderEmail);
            _logger.LogInformation("Subject: {Subject}", subject);
            _logger.LogInformation("Message: {Message}", message);
            _logger.LogInformation("----------------------------------------");
            return;
        }

        try
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Portfolio Contact Form", smtpUser));
            email.To.Add(new MailboxAddress("Shahid Hussain", receiverEmail));
            email.ReplyTo.Add(new MailboxAddress(senderName, senderEmail));
            email.Subject = $"[Portfolio Inquiry] {subject}";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <h3>New message from your Portfolio website</h3>
                    <p><strong>Name:</strong> {senderName}</p>
                    <p><strong>Email:</strong> {senderEmail}</p>
                    <p><strong>Subject:</strong> {subject}</p>
                    <hr />
                    <p>{message.Replace("\n", "<br/>")}</p>
                ",
                TextBody = $"From: {senderName} ({senderEmail})\nSubject: {subject}\n\n{message}"
            };

            email.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            var port = int.TryParse(smtpPortString, out var parsedPort) ? parsedPort : 587;

            await client.ConnectAsync(smtpHost, port, SecureSocketOptions.StartTls, cancellationToken);
            await client.AuthenticateAsync(smtpUser, smtpPass, cancellationToken);
            await client.SendAsync(email, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);

            _logger.LogInformation("Contact notification email delivered successfully for {SenderEmail}.", senderEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deliver contact notification email for {SenderEmail}.", senderEmail);
        }
    }
}
