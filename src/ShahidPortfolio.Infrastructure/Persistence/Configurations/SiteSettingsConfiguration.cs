using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShahidPortfolio.Domain.Entities;

namespace ShahidPortfolio.Infrastructure.Persistence.Configurations;

public class SiteSettingsConfiguration : IEntityTypeConfiguration<SiteSettings>
{
    public void Configure(EntityTypeBuilder<SiteSettings> builder)
    {
        builder.ToTable("SiteSettings");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.FullName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.ProfessionalTitle)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(s => s.OneLineBio)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(s => s.AboutSummary)
            .HasMaxLength(2000);

        builder.Property(s => s.AvailabilityStatus)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.CurrentLocation)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.CvUrl)
            .HasMaxLength(500)
            .IsRequired();
    }
}
