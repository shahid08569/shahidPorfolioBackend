using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShahidPortfolio.Domain.Entities;

namespace ShahidPortfolio.Infrastructure.Persistence.Configurations;

public class ExperienceConfiguration : IEntityTypeConfiguration<Experience>
{
    public void Configure(EntityTypeBuilder<Experience> builder)
    {
        builder.ToTable("Experiences");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Company)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(e => e.Role)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(e => e.Location)
            .HasMaxLength(100);

        builder.Property(e => e.EmploymentType)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(e => e.AchievementsJson)
            .IsRequired();

        builder.Property(e => e.TechStackJson)
            .IsRequired();

        builder.HasIndex(e => e.DisplayOrder);
    }
}
