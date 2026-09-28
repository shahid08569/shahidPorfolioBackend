using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShahidPortfolio.Domain.Entities;

namespace ShahidPortfolio.Infrastructure.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(p => p.Slug)
            .HasMaxLength(160)
            .IsRequired();

        builder.HasIndex(p => p.Slug)
            .IsUnique();

        builder.Property(p => p.Summary)
            .HasMaxLength(350)
            .IsRequired();

        builder.Property(p => p.ThumbnailUrl)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(p => p.BannerUrl)
            .HasMaxLength(500);

        builder.Property(p => p.LiveUrl)
            .HasMaxLength(500);

        builder.Property(p => p.GithubUrl)
            .HasMaxLength(500);

        builder.Property(p => p.TechStackJson)
            .IsRequired();

        builder.HasIndex(p => p.IsFeatured);
        builder.HasIndex(p => p.DisplayOrder);
    }
}
