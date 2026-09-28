using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShahidPortfolio.Domain.Entities;

namespace ShahidPortfolio.Infrastructure.Persistence.Configurations;

public class TestimonialConfiguration : IEntityTypeConfiguration<Testimonial>
{
    public void Configure(EntityTypeBuilder<Testimonial> builder)
    {
        builder.ToTable("Testimonials");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.ClientName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.Role)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.Company)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.AvatarUrl)
            .HasMaxLength(500);

        builder.Property(t => t.Content)
            .HasMaxLength(1500)
            .IsRequired();

        builder.Property(t => t.LinkedInUrl)
            .HasMaxLength(500);

        builder.HasIndex(t => t.IsActive);
        builder.HasIndex(t => t.DisplayOrder);
    }
}
