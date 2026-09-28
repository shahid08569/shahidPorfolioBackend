using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Application.Common.Models;
using ShahidPortfolio.Domain.Entities;
using ShahidPortfolio.Domain.Enums;

namespace ShahidPortfolio.Application.Features.Blog.Commands;

public record CreateBlogPostCommand(
    string Title,
    string Slug,
    string Summary,
    string ContentMarkdown,
    string? CoverImageUrl,
    string TagsJson,
    int ReadTimeMinutes,
    BlogStatus Status
) : IRequest<ApiResponse<Guid>>;

public class CreateBlogPostCommandValidator : AbstractValidator<CreateBlogPostCommand>
{
    public CreateBlogPostCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Slug).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Summary).NotEmpty().MaximumLength(500);
        RuleFor(x => x.ContentMarkdown).NotEmpty();
    }
}

public class CreateBlogPostCommandHandler : IRequestHandler<CreateBlogPostCommand, ApiResponse<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateBlogPostCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(CreateBlogPostCommand request, CancellationToken cancellationToken)
    {
        var slugExists = await _context.BlogPosts
            .AnyAsync(b => b.Slug.ToLower() == request.Slug.ToLower(), cancellationToken);

        if (slugExists)
        {
            return ApiResponse<Guid>.Fail("An article with this slug already exists.");
        }

        var post = new BlogPost
        {
            Title = request.Title,
            Slug = request.Slug.Trim().ToLowerInvariant(),
            Summary = request.Summary,
            ContentMarkdown = request.ContentMarkdown,
            CoverImageUrl = request.CoverImageUrl,
            TagsJson = string.IsNullOrWhiteSpace(request.TagsJson) ? "[]" : request.TagsJson,
            ReadTimeMinutes = request.ReadTimeMinutes <= 0 ? 5 : request.ReadTimeMinutes,
            Status = request.Status,
            PublishedAtUtc = request.Status == BlogStatus.Published ? DateTime.UtcNow : null,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _context.BlogPosts.AddAsync(post, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.Ok(post.Id, "Article created successfully.");
    }
}

public record UpdateBlogPostCommand(
    Guid Id,
    string Title,
    string Slug,
    string Summary,
    string ContentMarkdown,
    string? CoverImageUrl,
    string TagsJson,
    int ReadTimeMinutes,
    BlogStatus Status
) : IRequest<ApiResponse<bool>>;

public class UpdateBlogPostCommandHandler : IRequestHandler<UpdateBlogPostCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _context;

    public UpdateBlogPostCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(UpdateBlogPostCommand request, CancellationToken cancellationToken)
    {
        var post = await _context.BlogPosts.FindAsync(new object[] { request.Id }, cancellationToken);
        if (post == null)
        {
            return ApiResponse<bool>.Fail("Article not found.");
        }

        var slugExists = await _context.BlogPosts
            .AnyAsync(b => b.Id != request.Id && b.Slug.ToLower() == request.Slug.ToLower(), cancellationToken);

        if (slugExists)
        {
            return ApiResponse<bool>.Fail("Another article with this slug already exists.");
        }

        post.Title = request.Title;
        post.Slug = request.Slug.Trim().ToLowerInvariant();
        post.Summary = request.Summary;
        post.ContentMarkdown = request.ContentMarkdown;
        post.CoverImageUrl = request.CoverImageUrl;
        post.TagsJson = string.IsNullOrWhiteSpace(request.TagsJson) ? "[]" : request.TagsJson;
        post.ReadTimeMinutes = request.ReadTimeMinutes <= 0 ? 5 : request.ReadTimeMinutes;
        
        if (post.Status != BlogStatus.Published && request.Status == BlogStatus.Published && post.PublishedAtUtc == null)
        {
            post.PublishedAtUtc = DateTime.UtcNow;
        }
        post.Status = request.Status;
        post.UpdatedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Ok(true, "Article updated successfully.");
    }
}

public record DeleteBlogPostCommand(Guid Id) : IRequest<ApiResponse<bool>>;

public class DeleteBlogPostCommandHandler : IRequestHandler<DeleteBlogPostCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _context;

    public DeleteBlogPostCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(DeleteBlogPostCommand request, CancellationToken cancellationToken)
    {
        var post = await _context.BlogPosts.FindAsync(new object[] { request.Id }, cancellationToken);
        if (post == null)
        {
            return ApiResponse<bool>.Fail("Article not found.");
        }

        _context.BlogPosts.Remove(post);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Ok(true, "Article deleted successfully.");
    }
}
