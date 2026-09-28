using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShahidPortfolio.Domain.Entities;

namespace ShahidPortfolio.Infrastructure.Persistence.Configurations;

public class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessage>
{
    public void Configure(EntityTypeBuilder<ContactMessage> builder)
    {
        builder.ToTable("ContactMessages");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Email)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(c => c.Subject)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Message)
            .HasMaxLength(2500)
            .IsRequired();

        builder.Property(c => c.IpAddress)
            .HasMaxLength(45);

        builder.HasIndex(c => c.IsRead);
        builder.HasIndex(c => c.ReceivedAtUtc);
    }
}
