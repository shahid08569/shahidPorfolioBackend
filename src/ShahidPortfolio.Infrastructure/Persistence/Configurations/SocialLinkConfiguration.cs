using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShahidPortfolio.Domain.Entities;

namespace ShahidPortfolio.Infrastructure.Persistence.Configurations;

public class SocialLinkConfiguration : IEntityTypeConfiguration<SocialLink>
{
    public void Configure(EntityTypeBuilder<SocialLink> builder)
    {
        builder.ToTable("SocialLinks");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Platform)
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(s => s.Url)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(s => s.IconKey)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(s => s.IsActive);
        builder.HasIndex(s => s.DisplayOrder);
    }
}
