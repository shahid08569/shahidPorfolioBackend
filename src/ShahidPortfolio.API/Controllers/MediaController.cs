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

    private string GetUploadsDirectory()
    {
        var candidatePaths = new[]
        {
            Path.Combine(_environment.ContentRootPath, "wwwroot", "uploads"),
            Path.Combine(Directory.GetCurrentDirectory(), "src", "ShahidPortfolio.API", "wwwroot", "uploads"),
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads")
        };

        var dir = candidatePaths.FirstOrDefault(Directory.Exists)
            ?? Path.Combine(_environment.ContentRootPath, "wwwroot", "uploads");

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        return dir;
    }

    private void MirrorFileToCandidates(string fileName, string sourcePath)
    {
        var candidatePaths = new[]
        {
            Path.Combine(_environment.ContentRootPath, "wwwroot", "uploads"),
            Path.Combine(Directory.GetCurrentDirectory(), "src", "ShahidPortfolio.API", "wwwroot", "uploads"),
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads")
        };

        foreach (var cand in candidatePaths)
        {
            if (Directory.Exists(cand))
            {
                var target = Path.Combine(cand, fileName);
                if (!string.Equals(Path.GetFullPath(target), Path.GetFullPath(sourcePath), StringComparison.OrdinalIgnoreCase))
                {
                    try { System.IO.File.Copy(sourcePath, target, true); } catch { }
                }
            }
        }
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

        var uploadsDir = GetUploadsDirectory();
        var fileName = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
        var filePath = Path.Combine(uploadsDir, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        MirrorFileToCandidates(fileName, filePath);

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

        var uploadsDir = GetUploadsDirectory();
        var fileName = "Shahid_Hussain_CV.pdf";
        var filePath = Path.Combine(uploadsDir, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        MirrorFileToCandidates(fileName, filePath);

        var relativeUrl = $"/uploads/{fileName}";

        var settings = await _context.SiteSettings.FirstOrDefaultAsync(cancellationToken);
        if (settings != null)
        {
            settings.CvUrl = relativeUrl;
        }
        else
        {
            settings = new ShahidPortfolio.Domain.Entities.SiteSettings
            {
                CvUrl = relativeUrl
            };
            await _context.SiteSettings.AddAsync(settings, cancellationToken);
        }
        await _context.SaveChangesAsync(cancellationToken);

        return Ok(ApiResponse<string>.Ok(relativeUrl, "CV uploaded and site settings updated successfully."));
    }

    [HttpPost("upload-photo")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadPhoto([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(ApiResponse<string>.Fail("No image file provided."));
        }

        var extension = Path.GetExtension(file.FileName);
        var allowedPhotoExts = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp" };
        if (string.IsNullOrEmpty(extension) || !allowedPhotoExts.Contains(extension))
        {
            return BadRequest(ApiResponse<string>.Fail("Photo must be an image format (.jpg, .jpeg, .png, .webp)."));
        }

        var uploadsDir = GetUploadsDirectory();
        var fileName = $"profile_{DateTime.UtcNow:yyyyMMddHHmmss}{extension.ToLowerInvariant()}";
        var filePath = Path.Combine(uploadsDir, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        MirrorFileToCandidates(fileName, filePath);

        var relativeUrl = $"/uploads/{fileName}";

        var settings = await _context.SiteSettings.FirstOrDefaultAsync(cancellationToken);
        if (settings != null)
        {
            settings.ProfilePhotoUrl = relativeUrl;
        }
        else
        {
            settings = new ShahidPortfolio.Domain.Entities.SiteSettings
            {
                ProfilePhotoUrl = relativeUrl
            };
            await _context.SiteSettings.AddAsync(settings, cancellationToken);
        }
        await _context.SaveChangesAsync(cancellationToken);

        return Ok(ApiResponse<string>.Ok(relativeUrl, "Profile photo uploaded and updated successfully."));
    }

    [HttpPost("upload-hero-image")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadHeroImage([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(ApiResponse<string>.Fail("No image file provided."));
        }

        var extension = Path.GetExtension(file.FileName);
        var allowedPhotoExts = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp" };
        if (string.IsNullOrEmpty(extension) || !allowedPhotoExts.Contains(extension))
        {
            return BadRequest(ApiResponse<string>.Fail("Hero image must be an image format (.jpg, .jpeg, .png, .webp)."));
        }

        var uploadsDir = GetUploadsDirectory();
        var fileName = $"hero_{DateTime.UtcNow:yyyyMMddHHmmss}{extension.ToLowerInvariant()}";
        var filePath = Path.Combine(uploadsDir, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        MirrorFileToCandidates(fileName, filePath);

        var relativeUrl = $"/uploads/{fileName}";

        var settings = await _context.SiteSettings.FirstOrDefaultAsync(cancellationToken);
        if (settings != null)
        {
            settings.HeroImageUrl = relativeUrl;
        }
        else
        {
            settings = new ShahidPortfolio.Domain.Entities.SiteSettings
            {
                HeroImageUrl = relativeUrl
            };
            await _context.SiteSettings.AddAsync(settings, cancellationToken);
        }
        await _context.SaveChangesAsync(cancellationToken);

        return Ok(ApiResponse<string>.Ok(relativeUrl, "Hero card image uploaded and updated successfully."));
    }

    [HttpGet("cv")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public IActionResult DownloadCv()
    {
        var candidatePaths = new[]
        {
            Path.Combine(GetUploadsDirectory(), "Shahid_Hussain_CV.pdf"),
            Path.Combine(_environment.ContentRootPath, "wwwroot", "uploads", "Shahid_Hussain_CV.pdf"),
            Path.Combine(Directory.GetCurrentDirectory(), "src", "ShahidPortfolio.API", "wwwroot", "uploads", "Shahid_Hussain_CV.pdf"),
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "Shahid_Hussain_CV.pdf")
        };

        var filePath = candidatePaths.FirstOrDefault(System.IO.File.Exists);

        if (string.IsNullOrEmpty(filePath))
        {
            return NotFound(ApiResponse<string>.Fail("CV file has not been uploaded yet."));
        }

        var stream = System.IO.File.OpenRead(filePath);
        return File(stream, "application/pdf", "Shahid_Hussain_CV.pdf", enableRangeProcessing: true);
    }
}
