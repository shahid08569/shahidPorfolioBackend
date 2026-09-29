using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Application.Features.Admin.Commands;
using ShahidPortfolio.Application.Features.Admin.Queries;
using ShahidPortfolio.Application.Features.Blog.Commands;
using ShahidPortfolio.Application.Features.Experiences.Commands;
using ShahidPortfolio.Application.Features.Projects.Commands.CreateProject;
using ShahidPortfolio.Application.Features.Projects.Commands.DeleteProject;
using ShahidPortfolio.Application.Features.Projects.Commands.UpdateProject;
using ShahidPortfolio.Application.Features.Skills.Commands;

namespace ShahidPortfolio.API.Controllers;

[Authorize]
public class AdminController : BaseApiController
{
    // ==========================================
    // PROJECTS MANAGEMENT
    // ==========================================
    [HttpPost("projects")]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("projects/{id:guid}")]
    public async Task<IActionResult> UpdateProject(Guid id, [FromBody] UpdateProjectCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest(ApiResponse<bool>.Fail("ID mismatch."));
        var result = await Mediator.Send(command, cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("projects/{id:guid}")]
    public async Task<IActionResult> DeleteProject(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteProjectCommand(id), cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    // ==========================================
    // SKILLS MANAGEMENT
    // ==========================================
    [HttpPost("skills")]
    public async Task<IActionResult> CreateSkill([FromBody] CreateSkillCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("skills/{id:guid}")]
    public async Task<IActionResult> UpdateSkill(Guid id, [FromBody] UpdateSkillCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest(ApiResponse<bool>.Fail("ID mismatch."));
        var result = await Mediator.Send(command, cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("skills/{id:guid}")]
    public async Task<IActionResult> DeleteSkill(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteSkillCommand(id), cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    // ==========================================
    // EXPERIENCES MANAGEMENT
    // ==========================================
    [HttpPost("experiences")]
    public async Task<IActionResult> CreateExperience([FromBody] CreateExperienceCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("experiences/{id:guid}")]
    public async Task<IActionResult> UpdateExperience(Guid id, [FromBody] UpdateExperienceCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest(ApiResponse<bool>.Fail("ID mismatch."));
        var result = await Mediator.Send(command, cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("experiences/{id:guid}")]
    public async Task<IActionResult> DeleteExperience(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteExperienceCommand(id), cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    // ==========================================
    // BLOG MANAGEMENT
    // ==========================================
    [HttpPost("blog")]
    public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("blog/{id:guid}")]
    public async Task<IActionResult> UpdateBlogPost(Guid id, [FromBody] UpdateBlogPostCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest(ApiResponse<bool>.Fail("ID mismatch."));
        var result = await Mediator.Send(command, cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("blog/{id:guid}")]
    public async Task<IActionResult> DeleteBlogPost(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteBlogPostCommand(id), cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    // ==========================================
    // CONTACT MESSAGES
    // ==========================================
    [HttpGet("messages")]
    public async Task<IActionResult> GetMessages(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetContactMessagesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPut("messages/{id:guid}/read")]
    public async Task<IActionResult> MarkMessageAsRead(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new MarkContactMessageAsReadCommand(id), cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("messages/{id:guid}")]
    public async Task<IActionResult> DeleteMessage(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteContactMessageCommand(id), cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    // ==========================================
    // SITE SETTINGS
    // ==========================================
    [HttpPut("settings")]
    public async Task<IActionResult> UpdateSettings([FromBody] UpdateSiteSettingsCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    // ==========================================
    // TESTIMONIALS & ENDORSEMENTS MANAGEMENT
    // ==========================================
    [HttpGet("testimonials")]
    public async Task<IActionResult> GetAllTestimonials(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ShahidPortfolio.Application.Features.Testimonials.Commands.GetAdminTestimonialsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPut("testimonials/{id:guid}/approve")]
    public async Task<IActionResult> ApproveTestimonial(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ShahidPortfolio.Application.Features.Testimonials.Commands.ApproveTestimonialCommand(id), cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("testimonials/{id:guid}/reject")]
    public async Task<IActionResult> RejectTestimonial(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ShahidPortfolio.Application.Features.Testimonials.Commands.RejectTestimonialCommand(id), cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPost("testimonials")]
    public async Task<IActionResult> CreateTestimonial([FromBody] ShahidPortfolio.Application.Features.Testimonials.Commands.CreateAdminTestimonialCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("testimonials/{id:guid}")]
    public async Task<IActionResult> UpdateTestimonial(Guid id, [FromBody] ShahidPortfolio.Application.Features.Testimonials.Commands.UpdateAdminTestimonialCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest(ApiResponse<bool>.Fail("ID mismatch."));
        var result = await Mediator.Send(command, cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("testimonials/{id:guid}")]
    public async Task<IActionResult> DeleteTestimonial(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ShahidPortfolio.Application.Features.Testimonials.Commands.DeleteTestimonialCommand(id), cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    // ==========================================
    // CERTIFICATES MANAGEMENT
    // ==========================================
    [HttpGet("certificates")]
    public async Task<IActionResult> GetAllCertificates(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ShahidPortfolio.Application.Features.Certificates.Queries.GetCertificatesQuery(IncludeInactive: true), cancellationToken);
        return Ok(result);
    }

    [HttpPost("certificates")]
    public async Task<IActionResult> CreateCertificate([FromBody] ShahidPortfolio.Application.Features.Certificates.Commands.CreateCertificateCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("certificates/{id:guid}")]
    public async Task<IActionResult> UpdateCertificate(Guid id, [FromBody] ShahidPortfolio.Application.Features.Certificates.Commands.UpdateCertificateCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest(ApiResponse<bool>.Fail("ID mismatch."));
        var result = await Mediator.Send(command, cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("certificates/{id:guid}")]
    public async Task<IActionResult> DeleteCertificate(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ShahidPortfolio.Application.Features.Certificates.Commands.DeleteCertificateCommand(id), cancellationToken);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}
