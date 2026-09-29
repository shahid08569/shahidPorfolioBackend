using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Application.Common.Models;

namespace ShahidPortfolio.API.Controllers;

[Authorize]
public class MediaController : BaseApiController
{
    private readonly IWebHostEnvironment _environment;
    private readonly IApplicationDbContext _context;

    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp", ".svg", ".pdf"
    };

    public MediaController(IWebHostEnvironment environment, IApplicationDbContext context)
    {
        _environment = environment;
        _context = context;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(ApiResponse<string>.Fail("No file provided."));
        }

        if (file.Length > 10 * 1024 * 1024) // 10 MB limit
        {
            return BadRequest(ApiResponse<string>.Fail("File size exceeds 10 MB limit."));
        }

        var extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Contains(extension))
        {
            return BadRequest(ApiResponse<string>.Fail($"Unsupported file format. Allowed: {string.Join(", ", AllowedExtensions)}"));
        }

        var webRoot = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var uploadsDir = Path.Combine(webRoot, "uploads");

        if (!Directory.Exists(uploadsDir))
        {
            Directory.CreateDirectory(uploadsDir);
        }

        var fileName = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
        var filePath = Path.Combine(uploadsDir, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        var relativeUrl = $"/uploads/{fileName}";
        return Ok(ApiResponse<string>.Ok(relativeUrl, "File uploaded successfully."));
    }

    [HttpPost("upload-cv")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadCv([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(ApiResponse<string>.Fail("No file provided."));
        }

        var extension = Path.GetExtension(file.FileName);
        if (!string.Equals(extension, ".pdf", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(ApiResponse<string>.Fail("CV must be a PDF document (.pdf)."));
        }

        var webRoot = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var uploadsDir = Path.Combine(webRoot, "uploads");

        if (!Directory.Exists(uploadsDir))
        {
            Directory.CreateDirectory(uploadsDir);
        }

        var fileName = "Shahid_Hussain_CV.pdf";
        var filePath = Path.Combine(uploadsDir, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        var relativeUrl = $"/uploads/{fileName}";

        var settings = await _context.SiteSettings.FirstOrDefaultAsync(cancellationToken);
        if (settings != null)
        {
            settings.CvUrl = relativeUrl;
            await _context.SaveChangesAsync(cancellationToken);
        }

        return Ok(ApiResponse<string>.Ok(relativeUrl, "CV uploaded and site settings updated successfully."));
    }

    [HttpGet("cv")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public IActionResult DownloadCv()
    {
        var webRoot = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var filePath = Path.Combine(webRoot, "uploads", "Shahid_Hussain_CV.pdf");

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound(ApiResponse<string>.Fail("CV file has not been uploaded yet."));
        }

        var stream = System.IO.File.OpenRead(filePath);
        return File(stream, "application/pdf", "Shahid_Hussain_CV.pdf", enableRangeProcessing: true);
    }
}
