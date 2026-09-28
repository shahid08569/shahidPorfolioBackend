using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShahidPortfolio.Domain.Entities;

namespace ShahidPortfolio.Infrastructure.Persistence.Configurations;

public class EducationConfiguration : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.ToTable("Educations");

        builder.HasKey(ed => ed.Id);

        builder.Property(ed => ed.Institution)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(ed => ed.Degree)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(ed => ed.FieldOfStudy)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(ed => ed.GradeOrHonors)
            .HasMaxLength(80);

        builder.HasIndex(ed => ed.DisplayOrder);
    }
}
