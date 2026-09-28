using Microsoft.EntityFrameworkCore;
using ShahidPortfolio.Infrastructure;
using ShahidPortfolio.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add Infrastructure & Persistence Services
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Shahid Portfolio API", Version = "v1" });
});

var app = builder.Build();

// Automatically apply migrations and seed database at startup
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shahid Portfolio API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
