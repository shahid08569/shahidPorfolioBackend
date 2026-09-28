using Microsoft.EntityFrameworkCore;
using ShahidPortfolio.Domain.Entities;

namespace ShahidPortfolio.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<AdminUser> AdminUsers { get; }
    DbSet<SiteSettings> SiteSettings { get; }
    DbSet<Project> Projects { get; }
    DbSet<Skill> Skills { get; }
    DbSet<Experience> Experiences { get; }
    DbSet<Education> Educations { get; }
    DbSet<Testimonial> Testimonials { get; }
    DbSet<BlogPost> BlogPosts { get; }
    DbSet<ContactMessage> ContactMessages { get; }
    DbSet<SocialLink> SocialLinks { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
