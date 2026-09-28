using Microsoft.EntityFrameworkCore;
using ShahidPortfolio.Domain.Entities;
using ShahidPortfolio.Domain.Enums;
using ShahidPortfolio.Infrastructure.Security;

namespace ShahidPortfolio.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // 1. Seed Admin User
        if (!await context.AdminUsers.AnyAsync())
        {
            PasswordHasher.CreatePasswordHash("Admin@Portfolio2026!", out string hash, out string salt);

            var admin = new AdminUser
            {
                Username = "admin",
                Email = "shahidhussaain08569@gmail.com",
                PasswordHash = hash,
                PasswordSalt = salt,
                CreatedAtUtc = DateTime.UtcNow
            };

            await context.AdminUsers.AddAsync(admin);
        }

        // 2. Seed Site Settings
        if (!await context.SiteSettings.AnyAsync())
        {
            var settings = new SiteSettings
            {
                FullName = "Shahid Hussain",
                ProfessionalTitle = "Full-Stack .NET & Angular Developer",
                OneLineBio = "Architecting robust enterprise solutions with ASP.NET Core Clean Architecture and reactive Angular applications.",
                AboutSummary = "I am a dedicated full-stack software engineer with deep expertise in .NET, C#, EF Core, and Angular. I build fast, secure, scalable distributed systems and fluid, responsive user interfaces that deliver business results.",
                AvailabilityStatus = "Open to full-time engineering roles",
                CurrentLocation = "Pakistan",
                CvUrl = "/uploads/Shahid_Hussain_CV.pdf",
                UpdatedAtUtc = DateTime.UtcNow
            };

            await context.SiteSettings.AddAsync(settings);
        }

        // 3. Seed Skills
        if (!await context.Skills.AnyAsync())
        {
            var skills = new List<Skill>
            {
                // Frontend
                new() { Name = "Angular", Category = SkillCategory.Frontend, IconKey = "angular", Proficiency = 92, IsTopSkill = true, DisplayOrder = 1 },
                new() { Name = "TypeScript", Category = SkillCategory.Frontend, IconKey = "typescript", Proficiency = 90, IsTopSkill = true, DisplayOrder = 2 },
                new() { Name = "RxJS & Signals", Category = SkillCategory.Frontend, IconKey = "rxjs", Proficiency = 88, IsTopSkill = false, DisplayOrder = 3 },
                new() { Name = "SCSS & CSS Variables", Category = SkillCategory.Frontend, IconKey = "sass", Proficiency = 90, IsTopSkill = false, DisplayOrder = 4 },
                new() { Name = "Angular SSR", Category = SkillCategory.Frontend, IconKey = "server", Proficiency = 85, IsTopSkill = false, DisplayOrder = 5 },

                // Backend
                new() { Name = "C# & .NET 8 / 10", Category = SkillCategory.Backend, IconKey = "dotnet", Proficiency = 95, IsTopSkill = true, DisplayOrder = 6 },
                new() { Name = "ASP.NET Core Web API", Category = SkillCategory.Backend, IconKey = "api", Proficiency = 94, IsTopSkill = true, DisplayOrder = 7 },
                new() { Name = "Entity Framework Core", Category = SkillCategory.Backend, IconKey = "database", Proficiency = 92, IsTopSkill = true, DisplayOrder = 8 },
                new() { Name = "Clean Architecture & CQRS", Category = SkillCategory.Backend, IconKey = "layers", Proficiency = 92, IsTopSkill = true, DisplayOrder = 9 },
                new() { Name = "MediatR & FluentValidation", Category = SkillCategory.Backend, IconKey = "check-circle", Proficiency = 90, IsTopSkill = false, DisplayOrder = 10 },

                // Database
                new() { Name = "Microsoft SQL Server", Category = SkillCategory.Database, IconKey = "sql-server", Proficiency = 90, IsTopSkill = true, DisplayOrder = 11 },
                new() { Name = "PostgreSQL", Category = SkillCategory.Database, IconKey = "postgresql", Proficiency = 85, IsTopSkill = false, DisplayOrder = 12 },
                new() { Name = "Redis Caching", Category = SkillCategory.Database, IconKey = "redis", Proficiency = 80, IsTopSkill = false, DisplayOrder = 13 },

                // Tools & DevOps
                new() { Name = "Git & GitHub", Category = SkillCategory.ToolsDevOps, IconKey = "git", Proficiency = 92, IsTopSkill = true, DisplayOrder = 14 },
                new() { Name = "Docker & Containers", Category = SkillCategory.ToolsDevOps, IconKey = "docker", Proficiency = 85, IsTopSkill = false, DisplayOrder = 15 },
                new() { Name = "GitHub Actions CI/CD", Category = SkillCategory.ToolsDevOps, IconKey = "workflow", Proficiency = 82, IsTopSkill = false, DisplayOrder = 16 },
                new() { Name = "xUnit & Integration Testing", Category = SkillCategory.ToolsDevOps, IconKey = "check", Proficiency = 88, IsTopSkill = false, DisplayOrder = 17 }
            };

            await context.Skills.AddRangeAsync(skills);
        }

        // 4. Seed Social Links
        if (!await context.SocialLinks.AnyAsync())
        {
            var socialLinks = new List<SocialLink>
            {
                new() { Platform = "GitHub", Url = "https://github.com/shahid08569", IconKey = "github", DisplayOrder = 1, IsActive = true },
                new() { Platform = "LinkedIn", Url = "https://linkedin.com/in/shahid-hussain", IconKey = "linkedin", DisplayOrder = 2, IsActive = true },
                new() { Platform = "Email", Url = "mailto:shahidhussaain08569@gmail.com", IconKey = "mail", DisplayOrder = 3, IsActive = true }
            };

            await context.SocialLinks.AddRangeAsync(socialLinks);
        }

        // 5. Seed Initial Showcase Project
        if (!await context.Projects.AnyAsync())
        {
            var project = new Project
            {
                Title = "Enterprise E-Commerce API & Management Portal",
                Slug = "enterprise-ecommerce-portal",
                Summary = "Scalable multi-tenant e-commerce system built with .NET Clean Architecture, EF Core, SQL Server, and an Angular admin dashboard.",
                ProblemStatement = "Traditional monolith e-commerce backends suffer from high database contention and coupled business logic during peak promotional flash sales.",
                SolutionStatement = "Engineered a decoupled Clean Architecture backend using CQRS and MediatR to isolate read and write workloads, integrated with Redis cache and optimistic concurrency in EF Core.",
                ArchitectureOverview = "Layered Clean Architecture: Domain Core -> Application CQRS Commands/Queries -> Infrastructure SQL Server Persistence -> Web API with JWT Auth and Rate Limiting.",
                KeyMetrics = "Maintained sub-90ms response times under 5,000 concurrent product catalog queries; achieved 99.9% uptime during load testing.",
                LessonsLearned = "Careful projection with EF Core .Select() eliminates unnecessary column fetches and reduces network egress significantly.",
                ThumbnailUrl = "/assets/placeholders/project-ecommerce.jpg",
                BannerUrl = "/assets/placeholders/project-ecommerce-banner.jpg",
                LiveUrl = "https://github.com/shahid08569/shahidPorfolioBackend",
                GithubUrl = "https://github.com/shahid08569/shahidPorfolioBackend",
                TechStackJson = "[\"ASP.NET Core\", \"Angular\", \"SQL Server\", \"Clean Architecture\", \"MediatR\", \"EF Core\"]",
                Status = ProjectStatus.Completed,
                IsFeatured = true,
                IsCaseStudy = true,
                DisplayOrder = 1,
                CreatedAtUtc = DateTime.UtcNow
            };

            await context.Projects.AddAsync(project);
        }

        await context.SaveChangesAsync();
    }
}
