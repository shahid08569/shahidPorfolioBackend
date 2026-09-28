using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShahidPortfolio.Domain.Entities;

namespace ShahidPortfolio.Infrastructure.Persistence.Configurations;

public class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.ToTable("Skills");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(s => s.Category)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(s => s.IconKey)
            .HasMaxLength(50);

        builder.HasIndex(s => s.Category);
        builder.HasIndex(s => s.IsTopSkill);
        builder.HasIndex(s => s.DisplayOrder);
    }
}
