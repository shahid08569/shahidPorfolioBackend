using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using ShahidPortfolio.API.Middleware;
using ShahidPortfolio.Application;
using ShahidPortfolio.Infrastructure;
using ShahidPortfolio.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Application & Infrastructure Services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// 2. Configure CORS
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
    ?? new[] { "http://localhost:4200", "https://localhost:4200" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// 3. Configure Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // Contact form protection: Max 3 requests per 10 minutes per IP
    options.AddPolicy("contact-form-limit", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 3,
                Window = TimeSpan.FromMinutes(10),
                QueueLimit = 0
            }));
});

// 4. Add Controllers & Swagger Documentation
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "Shahid Portfolio Public & Admin API", 
        Version = "v1",
        Description = "Enterprise Clean Architecture Web API for Shahid's Developer Portfolio"
    });
});

var app = builder.Build();

// 5. Global Exception Middleware
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// 6. Automatically apply migrations & seed database at startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        if (context.Database.IsSqlServer())
        {
            await context.Database.MigrateAsync();
        }
        await DatabaseSeeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}

// 7. Configure HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shahid Portfolio API v1");
    });
}

app.UseHttpsRedirection();

app.UseCors("DefaultCorsPolicy");

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.Run();
