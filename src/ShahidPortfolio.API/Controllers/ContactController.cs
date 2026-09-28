using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Application.Features.Contact.Commands.SubmitContactMessage;

namespace ShahidPortfolio.API.Controllers;

public class ContactController : BaseApiController
{
    [HttpPost]
    [EnableRateLimiting("contact-form-limit")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> SubmitMessage([FromBody] ContactMessageRequest request)
    {
        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();

        var command = new SubmitContactMessageCommand(
            request.Name,
            request.Email,
            request.Subject,
            request.Message,
            request.Honeypot,
            clientIp
        );

        var result = await Mediator.Send(command);
        return Ok(result);
    }
}

public record ContactMessageRequest(
    string Name,
    string Email,
    string Subject,
    string Message,
    string? Honeypot = null
);
