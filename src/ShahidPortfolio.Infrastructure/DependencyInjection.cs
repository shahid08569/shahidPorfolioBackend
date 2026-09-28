using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShahidPortfolio.Application.Common.Interfaces;
using ShahidPortfolio.Infrastructure.Persistence;

namespace ShahidPortfolio.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? "Server=.\\SQLEXPRESS;Database=ShahidPortfolioDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;";

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString, b =>
                b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IEmailService, Services.EmailService>();

        return services;
    }
}
