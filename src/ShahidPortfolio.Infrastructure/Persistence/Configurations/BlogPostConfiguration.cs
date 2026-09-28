using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShahidPortfolio.Domain.Entities;

namespace ShahidPortfolio.Infrastructure.Persistence.Configurations;

public class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
{
    public void Configure(EntityTypeBuilder<BlogPost> builder)
    {
        builder.ToTable("BlogPosts");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(b => b.Slug)
            .HasMaxLength(220)
            .IsRequired();

        builder.HasIndex(b => b.Slug)
            .IsUnique();

        builder.Property(b => b.Summary)
            .HasMaxLength(400)
            .IsRequired();

        builder.Property(b => b.ContentMarkdown)
            .IsRequired();

        builder.Property(b => b.CoverImageUrl)
            .HasMaxLength(500);

        builder.Property(b => b.TagsJson)
            .IsRequired();

        builder.Property(b => b.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.HasIndex(b => b.Status);
        builder.HasIndex(b => b.PublishedAtUtc);
    }
}
